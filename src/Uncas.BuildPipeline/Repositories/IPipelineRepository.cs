namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;

    public interface IPipelineRepository
    {
        Pipeline GetPipeline(int pipelineId);
        
        IEnumerable<Pipeline> GetPipelines(int pageSize);
    }
}
