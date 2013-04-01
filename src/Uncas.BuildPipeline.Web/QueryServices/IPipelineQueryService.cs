using System.Collections.Generic;
using Uncas.BuildPipeline.Web.ViewModels;

namespace Uncas.BuildPipeline.Web.QueryServices
{
    public interface IPipelineQueryService
    {
        IEnumerable<PipelineListItemViewModel> GetPipelines(int pageSize);
    }
}