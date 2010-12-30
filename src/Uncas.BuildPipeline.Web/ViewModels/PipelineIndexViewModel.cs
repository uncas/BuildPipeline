namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System.Collections.Generic;

    public class PipelineIndexViewModel : BaseViewModel
    {
        public IEnumerable<PipelineViewModel> Pipelines { get; set; }
    }
}