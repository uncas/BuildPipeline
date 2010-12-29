namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.Models;

    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        public ActionResult Index()
        {
            var pipelines = new Repository().GetPipelines(BuildPageSize);
            var viewModels = PipelineMapper.MapToPipelineViewModels(pipelines);
            return View(viewModels);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
