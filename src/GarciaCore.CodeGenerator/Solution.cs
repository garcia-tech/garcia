using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class Solution
    {
        public Solution(string name, string folder, List<string> integrations, string defaultDatabaseServer)
        {
            Name = name;
            Folder = folder;
            Integrations = integrations;
            DefaultDatabaseServer = defaultDatabaseServer;
        }

        public string Name { get; set; }
        public string Folder { get; set; }
        public List<string> Integrations { get; }
        public string DefaultDatabaseServer { get; }
        public List<Project> Projects { get; set; } = new List<Project>();

        //protected virtual async Task<List<GenerationResult>> Generate(Item item)
        //{
        //    var generationResults = new List<GenerationResult>();

        //    foreach (var project in Projects)
        //    {
        //        var generationResult = await project.Generate(item);
        //        generationResults.AddRange(generationResult);
        //    }

        //    return generationResults;
        //}

        public virtual async Task<List<GenerationResult>> Generate(List<Item> items)
        {
            GeneratorRepository.Solution = this;
            var generationResults = new List<GenerationResult>();

            foreach (var item in items)
            {
                GeneratorRepository.AddItem(item);

                foreach (var project in Projects)
                {
                    var generationResult = await project.Generate(item);
                    generationResults.AddRange(generationResult);
                }

                foreach (var property in item.Properties.Where(x => x.InnerType != null))
                {
                    if (items.Count(x => x.Name.ToLowerInvariant() == property.InnerType.Name.ToLowerInvariant()) == 0)
                    {
                        generationResults.ForEach(x => x.Messages.Add($"Item {property.InnerType.Name} does not exist in item collection, possible build error."));
                    }
                }
            }

            foreach (var project in Projects)
            {
                var generationResult = await project.Generate();
                generationResults.AddRange(generationResult);
            }

            return generationResults;
        }
    }
}