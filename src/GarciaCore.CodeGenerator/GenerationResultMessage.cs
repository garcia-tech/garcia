namespace GarciaCore.CodeGenerator
{
    public class GenerationResultMessage
    {
        public GenerationResultMessage()
        {
        }

        public GenerationResultMessage(GenerationResultMessageType type, string message)
        {
            Type = type;
            Message = message;
        }

        public GenerationResultMessageType Type { get; set; }
        public string Message { get; set; }
    }
}