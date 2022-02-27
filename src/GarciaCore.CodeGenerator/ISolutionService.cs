namespace GarciaCore.CodeGenerator
{
    //public class SolutionTemplate
    //{
    //    public List<ProjectTemplate> Projects { get; set; } = new List<ProjectTemplate>();
    //}

    //public class ProjectTemplate
    //{
    //    public List<IGenerator> Generators { get; set; } = new List<IGenerator>();
    //    public List<ProjectTemplate> ProjectDependencies { get; set; } = new List<ProjectTemplate>();
    //}

    public interface ISolutionService
    {
        Solution CreateSolution(string solutionJson);
        string GetSolutionJson(Solution solution);
        string GetSampleJson();
        Solution CreateSampleSolution();
    }
}