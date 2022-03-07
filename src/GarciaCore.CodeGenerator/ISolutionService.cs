using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public interface ISolutionService
    {
        Task<Solution> CreateSolutionAsync(string solutionJson);
        Task<string> GetSolutionJsonAsync(Solution solution);
        Task<string> GetSampleJsonAsync();
        Task<Solution> CreateSampleSolutionAsync();
    }
}