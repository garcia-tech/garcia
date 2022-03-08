using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class SolutionGenerationResult
    {
        public SolutionGenerationResult(Solution solution, List<string> messages)
        {
            Solution = solution;
            Messages = messages;
        }

        public Solution Solution { get; set; }
        public List<string> Messages { get; set; }
    }
}