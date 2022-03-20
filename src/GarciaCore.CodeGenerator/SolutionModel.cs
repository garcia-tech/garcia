using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class SolutionModel
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public List<ProjectModel> Projects { get; set; }
    }
}