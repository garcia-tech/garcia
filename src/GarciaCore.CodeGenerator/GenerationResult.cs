using System.Collections.Generic;
using System.Linq;

namespace GarciaCore.CodeGenerator
{
    public class GenerationResult
    {
        public GenerationResult(string folder, IGenerator generator, string code, string file)
        {
            Folder = folder;
            Generator = generator;
            Code = code;
            File = file;
        }

        public string Folder { get; set; }
        public IGenerator Generator { get; set; }
        public string Code { get; set; }
        public List<GenerationResultMessage> Messages { get; set; } = new List<GenerationResultMessage>();
        public string AllMessages { get { return string.Join('\n', Messages.OrderByDescending(x => x.Type).Select(x => $"{x.Type.ToString()}: {x.Message}")); } }
        public string File { get; set; }
    }
}