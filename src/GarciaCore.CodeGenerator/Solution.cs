using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class Solution
    {
        public Solution(string name, string folder)
        {
            Name = name;
            Folder = folder;
        }

        public string Name { get; set; }
        public string Folder { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();

        public virtual async Task<List<GenerationResult>> Generate(Item item)
        {
            var generationResults = new List<GenerationResult>();

            foreach (var project in Projects)
            {
                var generationResult = await project.Generate(item);
                generationResults.AddRange(generationResult);
            }

            return generationResults;
        }
    }
}