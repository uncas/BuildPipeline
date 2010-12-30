namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.Models;

    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        private readonly Repository pipelineRepository;

        public HomeController()
        {
            this.pipelineRepository = new Repository();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var pipelines = this.pipelineRepository.GetPipelines(BuildPageSize);
            var viewModel = PipelineMapper.MapToPipelineIndexViewModel(pipelines);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Deploy(int pipelineId)
        {
            // TODO: Put this in application service:
            var pipeline = this.pipelineRepository.GetPipeline(pipelineId);
            string packagePath = pipeline.PackagePath;
            // TODO: Give proper feed back to user...
            var deploymentUtility = new DeploymentUtility();
            deploymentUtility.Deploy(packagePath, @"C:\temp\deploytest");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
