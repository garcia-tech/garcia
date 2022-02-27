using Newtonsoft.Json;

namespace GarciaCore.CodeGenerator
{
    public class SolutionService : ISolutionService
    {
        public Solution CreateSampleSolution()
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
            api.AddGenerator("Command", new CQRSWebApiCommandGenerator());
            api.AddGenerator("CommandHandler", new CQRSWebApiCommandHandlerGenerator());
            api.ProjectDependencies.Add(infrastructure);
            api.ProjectDependencies.Add(domain);
            solution.Projects.Add(api);

            return solution;
        }

        public Solution CreateSolution(string solutionJson)
        {
            var solution = JsonConvert.DeserializeObject<Solution>(solutionJson);

            if (solution == null)
            {
                throw new CodeGeneratorException("Cannot convert json to solution, please check sample json.");
            }

            return solution;
        }

        public string GetSampleJson()
        {
            var solution = CreateSampleSolution();
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public string GetSolutionJson(Solution solution)
        {
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}