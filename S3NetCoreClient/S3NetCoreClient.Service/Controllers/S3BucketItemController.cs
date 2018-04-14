using Amazon.S3;
using Amazon.S3.Model;
using log4net;
using Microsoft.AspNetCore.Mvc;
using S3NetCoreClient.Service.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace S3NetCoreClient.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/s3bucketItem")]
    public class S3BucketItemController : S3ControllerBase
    {
        public S3BucketItemController(IAmazonS3 client) : base(client)
        {
            Log = LogManager.GetLogger(GetType());
        }

        // GET: api/S3BucketItem
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromQuery]BucketItem item)
        {
            GetObjectResponse response = await S3Client.GetObjectAsync(item.BucketName, item.Key);
            HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            responseMessage.Content = new StreamContent(response.ResponseStream);
            //responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            //responseMessage.Content.Headers.ContentLength = response.ContentLength;
            foreach (string headerName in response.Headers.Keys)
            {
                responseMessage.Headers.Add(headerName, response.Headers[headerName]);
            }
            return responseMessage;
        }

        // GET: api/S3BucketItem/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/S3BucketItem
        [HttpPost]
        public void Post([FromBody]BucketItem item)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = item.BucketName;
            request.Key = item.Key;
            if (!string.IsNullOrEmpty(item.Base64Content))
            {
                request.ContentBody = item.Base64Content;
            }
            PutObjectResponse response = S3Client.PutObjectAsync(request).GetAwaiter().GetResult();
        }

        // PUT: api/S3BucketItem/5
        [HttpPut("{id}")]
        public void Put([FromBody]BucketItem item)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = item.BucketName;
            byte[] contentBytes = Convert.FromBase64String(item.Base64Content);
            request.ContentBody = item.Base64Content;
            request.ContentType = "application/csv";
            request.Key = item.Key;
            request.Metadata.Add("x-amz-meta-title", item.FileName);
            S3Client.PutObjectAsync(request);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}