using System.Collections.Generic;

namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class PipelineIndexViewModel
    {
        public IEnumerable<PipelineListItemViewModel> Pipelines { get; set; }
    }
}