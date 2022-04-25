using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Infrastructure.FileUpload.AmazonS3
{
    public class AmazonS3Settings
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketUrl { get; set; }
        public string BucketName { get; set; }
    }
}
