namespace Uncas.BuildPipeline.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);
        IEnumerable<Deployment> GetScheduledDeployments();
        IEnumerable<Deployment> GetDeployments(int pipelineId);

        ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId, 
            int environmentId, 
            DateTime scheduledStart);
    }
}