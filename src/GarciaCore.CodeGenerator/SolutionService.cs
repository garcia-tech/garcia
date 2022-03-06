using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class SolutionService : ISolutionService
    {
        public async Task<Solution> CreateSampleSolutionAsync()
        {
            var solution = new Solution("TestSolution", "c:\\files\\garciacoretest");

            var infrastructure = new Project("Infrastructure");
            infrastructure.AddGenerator("Entity", new EntityGenerator());
            solution.Projects.Add(infrastructure);

            var domain = new Project("Domain");
            domain.AddGenerator("Entity", new EntityGenerator());
            domain.AddGenerator("Repository", new RepositoryGenerator());
            domain.ProjectDependencies.Add(infrastructure);
            solution.Projects.Add(domain);

            var api = new Project("Api");
            api.AddGenerator("Controller", "Controllers", "ApiController", new CQRSWebApiControllerGenerator());
            api.AddGenerator("Command", new CQRSWebApiCreateCommandGenerator());
            api.AddGenerator("Command", new CQRSWebApiUpdateCommandGenerator());
            api.AddGenerator("Command", new CQRSWebApiDeleteCommandGenerator());
            api.AddGenerator("CommandHandler", new CQRSWebApiCreateCommandHandlerGenerator());
            api.AddGenerator("CommandHandler", new CQRSWebApiUpdateCommandHandlerGenerator());
            api.AddGenerator("CommandHandler", new CQRSWebApiDeleteCommandHandlerGenerator());
            api.ProjectDependencies.Add(infrastructure);
            api.ProjectDependencies.Add(domain);
            solution.Projects.Add(api);

            return solution;
        }

        public async Task<Solution> CreateSolutionAsync(string solutionJson)
        {
            var solution = JsonConvert.DeserializeObject<Solution>(solutionJson);

            if (solution == null)
            {
                throw new CodeGeneratorException("Cannot convert json to solution, please check sample json.");
            }

            return solution;
        }

        public async Task<string> GetSampleJsonAsync()
        {
            var solution = await CreateSampleSolutionAsync();
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<string> GetSolutionJsonAsync(Solution solution)
        {
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}