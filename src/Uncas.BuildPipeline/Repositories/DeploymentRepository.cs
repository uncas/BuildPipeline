using System;
using System.Collections.Generic;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Repositories
{
    public class DeploymentRepository : IDeploymentRepository
    {
        private readonly BuildPipelineConnection _connection =
            new BuildPipelineConnection();

        #region IDeploymentRepository Members

        public void AddDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            const string sql = @"
INSERT INTO Deployment
(Created, PipelineId, EnvironmentId)
VALUES
(@Created, @PipelineId, @EnvironmentId)

SET @DeploymentId = Scope_Identity()";
            var param =
                new {deployment.Created, deployment.PipelineId, deployment.EnvironmentId};
            int deploymentId = _connection.ExecuteAndGetGeneratedId(sql,
                                                                    param,
                                                                    "DeploymentId");
            deployment.ChangeId(deploymentId);
        }

        public IEnumerable<Deployment> GetByEnvironment(int environmentId)
        {
            const string sql = @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE EnvironmentId = @EnvironmentId
ORDER BY Created ASC";
            return _connection.Query<Deployment>(sql, new {environmentId});
        }

        public Deployment GetDeployment(int deploymentId)
        {
            const string sql = @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE DeploymentId = @DeploymentId
ORDER BY Created ASC";
            return
                _connection.Query<Deployment>(sql, new {deploymentId}).SingleOrDefault();
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            const string sql = @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            return _connection.Query<Deployment>(sql, new {pipelineId});
        }

        public IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo)
        {
            if (pagingInfo == null)
            {
                throw new ArgumentNullException("pagingInfo");
            }

            const string sql = @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE Started IS NULL 
    OR Completed IS NULL
ORDER BY Created ASC
LIMIT @PageSize";
            return _connection.Query<Deployment>(sql, new {pagingInfo.PageSize});
        }

        public void UpdateDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            const string sql = @"
UPDATE Deployment
SET Started = @Started
    , Completed = @Completed
WHERE DeploymentId = @id";
            var param = new {deployment.Started, deployment.Completed, deployment.Id};
            _connection.Execute(sql, param);
        }

        #endregion
    }
}