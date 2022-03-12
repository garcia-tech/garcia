using System.Collections.Generic;

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
        public List<string> Messages { get; set; } = new List<string>();
        public string AllMessages { get { return string.Join('\n', Messages); } }
        public string File { get; set; }
    }
}