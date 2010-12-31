namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        private readonly EnvironmentRepository environmentRepository;
        private readonly PipelineRepository pipelineRepository;

        public HomeController()
        {
            this.environmentRepository = new EnvironmentRepository();
            this.pipelineRepository = new PipelineRepository();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var pipelines = this.pipelineRepository.GetPipelines(BuildPageSize);
            var viewModel = PipelineMapper.MapToPipelineIndexViewModel(pipelines);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Deploy(int pipelineId)
        {
            var environments = this.environmentRepository.GetEnvironments();
            var viewModel = environments.Select(
                e => new EnvironmentViewModel
            {
                EnvironmentId = e.Id,
                EnvironmentName = e.EnvironmentName
            });
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Deploy(int environmentId, int pipelineId)
        {
            // TODO: Put this in application service:
            var pipeline = this.pipelineRepository.GetPipeline(pipelineId);
            Environment environment = this.environmentRepository.GetEnvironment(environmentId);
            string packagePath = pipeline.PackagePath;
            // TODO: Give proper feed back to user...
            var deploymentUtility = new DeploymentUtility();
            string workingDirectory = @"C:\temp\deploytest";
            deploymentUtility.Deploy(
                packagePath,
                workingDirectory,
                environment);
            return RedirectToAction("Deploy", new { PipelineId = pipelineId });
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
