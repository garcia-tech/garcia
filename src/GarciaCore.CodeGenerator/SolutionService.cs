using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class SolutionService : ISolutionService
    {
        public async Task<Solution> CreateSampleSolutionAsync()
        {
            var solution = new Solution("TestSolution", "c:\\files\\garciacoretest");

            var infrastructure = new Project("TestSolution.Infrastructure", ProjectType.ClassLibrary);
            infrastructure.AddGenerator("Repository", "Repository", new RepositoryGenerator());
            solution.Projects.Add(infrastructure);

            var domain = new Project("TestSolution.Domain", ProjectType.ClassLibrary);
            domain.AddGenerator("Entity", "Entity", new EntityGenerator());
            domain.ProjectDependencies.Add(infrastructure);
            solution.Projects.Add(domain);

            var api = new Project("TestSolution.Api", ProjectType.WebApi);
            api.AddGenerator("Controller", "Controllers", "ApiController", new CQRSWebApiControllerGenerator());
            api.ProjectDependencies.Add(infrastructure);
            api.ProjectDependencies.Add(domain);
            solution.Projects.Add(api);

            var application = new Project("TestSolution.Application", ProjectType.ClassLibrary);
            application.AddGenerator("Queries", "Queries", new CQRSWebApiQueryGenerator());
            application.AddGenerator("CreateCommand", "Commands", new CQRSWebApiCreateCommandGenerator());
            application.AddGenerator("UpdateCommand", "Commands", new CQRSWebApiUpdateCommandGenerator());
            application.AddGenerator("DeleteCommand", "Commands", new CQRSWebApiDeleteCommandGenerator());
            application.AddGenerator("CreateCommandHandler", "Commands", new CQRSWebApiCreateCommandHandlerGenerator());
            application.AddGenerator("UpdateCommandHandler", "Commands", new CQRSWebApiUpdateCommandHandlerGenerator());
            application.AddGenerator("DeleteCommandHandler", "Commands", new CQRSWebApiDeleteCommandHandlerGenerator());
            solution.Projects.Add(application);

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