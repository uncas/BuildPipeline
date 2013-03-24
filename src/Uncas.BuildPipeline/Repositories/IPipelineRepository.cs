using System.Collections.Generic;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public interface IPipelineRepository
    {
        void AddPipeline(Pipeline pipeline);
        Pipeline GetPipeline(int pipelineId);
        IEnumerable<Pipeline> GetPipelines(int pageSize);
    }
}