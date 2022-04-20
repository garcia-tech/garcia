using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class SolutionModel
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public List<string> Integrations { get; set; }
        public string DefaultDatabaseServer { get; set; }
        public bool OverwriteItemCode { get; set; }
        public bool OverwriteNonItemCode { get; set; }
    }
}