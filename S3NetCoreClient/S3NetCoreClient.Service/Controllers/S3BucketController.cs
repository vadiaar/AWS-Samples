using Amazon.S3;
using Amazon.S3.Model;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace S3NetCoreClient.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/s3bucket")]
    public class S3BucketController : S3ControllerBase
    {
        #region Constructor

        public S3BucketController(IAmazonS3 client) : base(client)
        {
            Log = LogManager.GetLogger(GetType());
        }

        #endregion Constructor

        #region Actions

        [HttpGet]
        public async Task<IEnumerable<S3Bucket>> GetAsync()
        {
            ListBucketsResponse listBucketsResponse = await S3Client.ListBucketsAsync();
            return listBucketsResponse.Buckets;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion Actions
    }
}