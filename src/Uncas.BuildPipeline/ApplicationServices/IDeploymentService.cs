namespace Uncas.BuildPipeline.ApplicationServices
{
    using System;
    using Uncas.BuildPipeline.ApplicationServices.Results;

    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);

        ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId, 
            int environmentId, 
            DateTime scheduledStart);
    }
}