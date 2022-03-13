using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public interface ISolutionService
    {
        Task<SolutionGenerationResult> CreateSolutionAsync(string solutionJson);
        Task<string> GetSolutionJsonAsync(Solution solution);
        Task<string> GetSampleJsonAsync();
        Task<Solution> CreateSampleSolutionAsync();
        Task<SolutionModel> CreateSampleSolution2Async();
    }
}