namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Models;

    public class EnvironmentRepository : IEnvironmentRepository
    {
        public IEnumerable<Environment> GetEnvironments()
        {
            // TODO: Make environments dependent on project:
            // TODO: Store environment information in database:
            var result = new List<Environment>();

            var integrationEnvironment = new Environment
            {
                Id = 1,
                EnvironmentName = "Integration"
            };
            integrationEnvironment.AddProperty(
                "website.destination.path",
                @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web");
            integrationEnvironment.AddProperty(
                "website.name",
                "BuildPipelineWeb");
            integrationEnvironment.AddProperty(
                "website.port",
                "876");
            result.Add(integrationEnvironment);

            var qaEnvironment = new Environment
            {
                Id = 2,
                EnvironmentName = "QA"
            };
            qaEnvironment.AddProperty(
                "website.destination.path",
                @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web.QA");
            qaEnvironment.AddProperty(
                "website.name",
                "BuildPipelineWeb-QA");
            qaEnvironment.AddProperty(
                "website.port",
                "872");
            result.Add(qaEnvironment);
            return result;
        }

        public Environment GetEnvironment(int environmentId)
        {
            return GetEnvironments().SingleOrDefault(
                e => e.Id == environmentId);
        }
    }
}