using Amazon.S3.Model;
using log4net;
using S3NetCoreClient.Service.Models;
using System;
using System.Collections.Generic;

namespace S3NetCoreClient.Client
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            ServiceManager manager = ServiceManager.Instance;
            Console.WriteLine("Welcome to Ravi's S3 Container");

            List<S3Bucket> buckets = manager.GetBuckets().GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Choose from below S3 containers ...");
            int index = 1;
            Dictionary<string, S3Bucket> bucketsByIndex = new Dictionary<string, S3Bucket>();
            foreach (S3Bucket bucket in buckets)
            {
                bucketsByIndex.Add(index.ToString(), bucket);
                Console.WriteLine($"Press {index} for {bucket.BucketName}");
                index++;
            }

            string bucketOption = Console.ReadLine();
            S3Bucket _currentBucket;
            while (!bucketsByIndex.TryGetValue(bucketOption, out _currentBucket))
            {
                Console.WriteLine("Invalid key. Please try again");
            }
            Console.WriteLine(_currentBucket.BucketName);

            foreach (ServiceManager.FileMeta fileMeta in manager.GetFileContentByKey())
            {
                BucketItem item = new BucketItem()
                {
                    BucketName = _currentBucket.BucketName,
                    Key = fileMeta.FolderKey,
                };
                if (manager.SaveItem(item))
                {
                    item = new BucketItem()
                    {
                        BucketName = _currentBucket.BucketName,
                        Key = $"{fileMeta.FolderKey}{fileMeta.FileKey}",
                        Base64Content = fileMeta.FileContentBase64String
                    };
                    if (!manager.SaveItem(item))
                    {
                        Console.WriteLine("Failed to upload file");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to create folder structure");
                }
            }

            Console.ReadLine();
        }
    }
}