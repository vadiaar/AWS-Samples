namespace S3NetCoreClient.Service.Models
{
    public class BucketItem
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string Base64Content { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}