namespace Garcia.Infrastructure.FileUpload.AzureBlob
{
    public class AzureFailedResponseException : Exception
    {
        public AzureFailedResponseException(int status) : base($"Azure request is failed. Status code: {status}.")
        {
        }
    }
}
