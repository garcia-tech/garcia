using System.Runtime.Serialization;

namespace GarciaCore.Exceptions.ElasticSearch
{
    [Serializable]
    public class DeleteDocumentException : Exception
    {
        public DeleteDocumentException()
        {
        }
        public DeleteDocumentException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }
        public DeleteDocumentException(string message) : base(message)
        {
        }
        public DeleteDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}