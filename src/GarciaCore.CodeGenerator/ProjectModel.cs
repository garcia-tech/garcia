using System;
using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Namespace { get; set; }
        public List<string> ProjectDependencies { get; set; }
        public List<ProjectGeneratorModel> Generators { get; set; }
        public string ProjectType { get; set; }
        protected internal Guid Uid { get; protected set; } = Guid.NewGuid();
    }
}