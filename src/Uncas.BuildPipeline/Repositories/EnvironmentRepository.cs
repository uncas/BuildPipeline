namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.Data;

    public class EnvironmentRepository : IEnvironmentRepository
    {
        public Environment GetEnvironment(int environmentId)
        {
            const int PageSize = 30;
            return GetEnvironments(new PagingInfo(PageSize)).SingleOrDefault(
                e => e.Id == environmentId);
        }

        public IEnumerable<Environment> GetEnvironments(PagingInfo pagingInfo)
        {
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

            var qualityAssuranceEnvironment = new Environment
                                                  {
                                                      Id = 2,
                                                      EnvironmentName = "QA"
                                                  };
            qualityAssuranceEnvironment.AddProperty(
                "website.destination.path",
                @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web.QA");
            qualityAssuranceEnvironment.AddProperty(
                "website.name",
                "BuildPipelineWeb-QA");
            qualityAssuranceEnvironment.AddProperty(
                "website.port",
                "872");
            result.Add(qualityAssuranceEnvironment);
            return result;
        }
    }
}