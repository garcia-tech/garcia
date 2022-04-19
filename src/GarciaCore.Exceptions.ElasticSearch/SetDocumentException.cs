using System.Runtime.Serialization;

namespace GarciaCore.Exceptions.ElasticSearch
{
    [Serializable]
    public class SetDocumentException : Exception
    {
        public SetDocumentException()
        {
        }
        public SetDocumentException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }
        public SetDocumentException(string message) : base(message)
        {
        }
        public SetDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}