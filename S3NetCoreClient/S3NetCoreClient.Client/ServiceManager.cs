using Amazon.S3.Model;
using Newtonsoft.Json;
using S3NetCoreClient.Service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace S3NetCoreClient.Client
{
    public class ServiceManager
    {
        #region Fields

        private static ServiceManager _instance;

        #endregion Fields

        #region Properties

        public static ServiceManager Instance
        {
            get
            {
                _instance = _instance ?? new ServiceManager();
                return _instance;
            }
        }

        public HttpClient Client { get; private set; }

        public S3Bucket CurrentBucket { get; set; }

        #endregion Properties

        #region Constructor

        protected ServiceManager()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:22831/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructor

        #region Inner Classes

        public class FileMeta
        {
            public string FolderKey { get; set; }
            public string FileKey { get; set; }
            public string FileContentBase64String { get; set; }
        }

        #endregion Inner Classes

        #region Public Methods

        public async Task<List<S3Bucket>> GetBuckets()
        {
            List<S3Bucket> buckets = new List<S3Bucket>();
            HttpResponseMessage response = await Client.GetAsync("api/s3bucket");
            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                buckets = JsonConvert.DeserializeObject<List<S3Bucket>>(jsonResult);
            }
            return buckets;
        }

        public List<FileMeta> GetFileContentByKey()
        {
            List<FileMeta> fileMetas = new List<FileMeta>();
            string sampleContentFolderPath = @"C:\Workspace\vadiaar\AWS-Samples\SampleContent";
            string[] filePaths = Directory.GetFiles(sampleContentFolderPath, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                string key = filePath.Replace(sampleContentFolderPath, string.Empty);
                key = key.Replace(fileName, string.Empty).Trim('\\');
                key = key.Replace('\\', '/');
                key = $"{key}/";
                Console.WriteLine(key);

                fileMetas.Add(new FileMeta()
                {
                    FolderKey = key,
                    FileKey = fileName,
                    FileContentBase64String = GetBase64StringFileContent(filePath)
                });
            }
            return fileMetas;
        }

        public string GetBase64StringFileContent(string filePath)
        {
            byte[] contentBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(contentBytes, 0, contentBytes.Length);
        }

        public bool SaveItem(BucketItem item)
        {
            List<S3Bucket> buckets = new List<S3Bucket>();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Client.PostAsync("api/s3bucketItem", content).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        #endregion Public Methods
    }
}