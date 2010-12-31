namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;
        private readonly IDeploymentService deploymentService;

        public HomeController()
        {
            this.environmentRepository = new EnvironmentRepository();
            this.pipelineRepository = new PipelineRepository();
            this.deploymentService = new DeploymentService(
                this.environmentRepository,
                this.pipelineRepository,
                new DeploymentUtility());
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
            this.deploymentService.Deploy(pipelineId, environmentId);

            // TODO: Give proper feed back to user:
            return RedirectToAction("Deploy", new { PipelineId = pipelineId });
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
