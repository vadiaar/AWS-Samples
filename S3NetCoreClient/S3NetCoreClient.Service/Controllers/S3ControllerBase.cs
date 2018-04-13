using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3NetCoreClient.Service.Controllers
{
    public class S3ControllerBase : Controller
    {
        protected IAmazonS3 S3Client { get; set; }

        protected S3ControllerBase(IAmazonS3 client)
        {
            S3Client = client;
        }

        
    }
}
