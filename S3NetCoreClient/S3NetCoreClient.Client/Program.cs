using Amazon.S3.Model;
using Newtonsoft.Json;
using S3NetCoreClient.Service.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace S3NetCoreClient.Client
{
    internal class Program
    {
        private static S3Bucket _currentBucket = null;   
        private static HttpClient _client;

        public static HttpClient Client
        {
            get
            {
                _client = _client ?? new HttpClient();
                return _client;
            }
        }
        
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ravi's S3 Container");
            Client.BaseAddress = new Uri("http://localhost:22831/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            List<S3Bucket> buckets = GetBuckets().GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Choose from below S3 containers ...");
            int index=1;
            Dictionary<string, S3Bucket> bucketsByIndex = new Dictionary<string, S3Bucket>();
            foreach (S3Bucket bucket in buckets)
            {
                bucketsByIndex.Add(index.ToString(), bucket);
                Console.WriteLine($"Press {index} for {bucket.BucketName}");
                index++;
                
            }
            
            string bucketOption = Console.ReadLine();                       
            while(!bucketsByIndex.TryGetValue(bucketOption, out _currentBucket))
            {
                Console.WriteLine("Invalid key. Please try again");
            }
            Console.WriteLine(_currentBucket.BucketName);
            Console.ReadLine();
            
        }

        private async static Task<List<S3Bucket>> GetBuckets()
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

        private void SaveItem(BucketItem item)
        {

        }
    }
}