using Amazon;

namespace Garcia.Infrastructure.FileUpload.AmazonS3
{
    public class AmazonS3Settings
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketUrl { get; set; }
        public string BucketName { get; set; }
        public RegionEndpoint RegionEndpoint { get; set; } = RegionEndpoint.EUCentral1;
        public string ServiceUrl { get; set; }
    }
}
