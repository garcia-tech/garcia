using System.Collections.Generic;
using System.Linq;

namespace GarciaCore.CodeGenerator
{
    public static class GeneratorRepository
    {
        private static List<IGenerator> Generators { get; set; } = new();
        public static List<Item> Items { get; set; } = new();

        public static void AddGenerator(IGenerator generator)
        {
            if (Generators.Count(x => x.Name == generator.Name) == 0)
            {
                Generators.Add(generator);
            }
        }

        public static bool ContainsGenerator(GeneratorType generatorType)
        {
            return Generators.Count(x => x.GeneratorType == generatorType) > 0;
        }

        public static string ApplicationModelDtoPostfix
        {
            get
            {
                return Generators.Count(x => x.GeneratorType == GeneratorType.Model) > 0 ? "Model" : string.Empty;
            }
        }

        public static void AddItem(Item item)
        {
            if (Generators.Count(x => x.Name == item.Name) == 0)
            {
                Items.Add(item);
            }
        }
    }
}