using System.Collections.Generic;

namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class PipelineIndexViewModel
    {
        public IEnumerable<PipelineViewModel> Pipelines { get; set; }
    }
}