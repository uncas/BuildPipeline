using System.Collections.Generic;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        #region IEnvironmentRepository Members

        public Environment GetEnvironment(int environmentId)
        {
            const int pageSize = 30;
            return GetEnvironments(new PagingInfo(pageSize)).SingleOrDefault(
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
            result.Add(integrationEnvironment);

            var qualityAssuranceEnvironment = new Environment
                {
                    Id = 2,
                    EnvironmentName = "QA"
                };
            result.Add(qualityAssuranceEnvironment);

            var productionEnvironment = new Environment
                {
                    Id = 3,
                    EnvironmentName = "Production"
                };
            result.Add(productionEnvironment);
            return result;
        }

        #endregion
    }
}