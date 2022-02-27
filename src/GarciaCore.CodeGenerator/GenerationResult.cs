using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class GenerationResult
    {
        public GenerationResult(string folder, IGenerator generator, string code)
        {
            Folder = folder;
            Generator = generator;
            Code = code;
        }

        public string Folder { get; set; }
        public IGenerator Generator { get; set; }
        public string Code { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}