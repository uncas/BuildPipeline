namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
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
            this.environmentRepository = Bootstrapper.Resolve<IEnvironmentRepository>();
            this.deploymentService = Bootstrapper.Resolve<IDeploymentService>();
            this.pipelineRepository = Bootstrapper.Resolve<IPipelineRepository>();
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
            var pipeline = this.pipelineRepository.GetPipeline(pipelineId);
            var deployments = this.deploymentService.GetDeployments(pipelineId);
            var environmentViewModels = environments.Select(
                e => new EnvironmentViewModel
            {
                EnvironmentId = e.Id,
                EnvironmentName = e.EnvironmentName
            });
            var pipelineViewModel = PipelineMapper.MapToPipelineViewModel(pipeline);
            var deployViewModel = new DeployViewModel
            {
                Environments = environmentViewModels,
                Deployments = deployments.Select(
                    d => new DeploymentViewModel
                    {
                        Created = d.Created,
                        Started = d.Started,
                        Completed = d.Completed,
                        EnvironmentName = environments.FirstOrDefault(
                            e => e.Id == d.EnvironmentId).EnvironmentName
                    }),
                Pipeline = pipelineViewModel
            };
            return View(deployViewModel);
        }

        [HttpPost]
        public ActionResult Deploy(int environmentId, int pipelineId)
        {
            this.deploymentService.ScheduleDeployment(pipelineId, environmentId);
            return RedirectToAction("Deploy", new { PipelineId = pipelineId });
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}