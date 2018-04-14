using Amazon.S3;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace S3NetCoreClient.Service.Controllers
{
    public class S3ControllerBase : Controller
    {
        #region Properties
        protected ILog Log { get; set; }
        protected IAmazonS3 S3Client { get; set; }
        #endregion

        #region Constructor

        protected S3ControllerBase(IAmazonS3 client)
        {
            S3Client = client;
        }

        #endregion Constructor
    }
}