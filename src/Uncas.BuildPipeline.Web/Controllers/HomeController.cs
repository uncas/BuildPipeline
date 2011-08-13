namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.ViewModels;
    using Uncas.Core.Data;

    /// <summary>
    /// Handles the front page.
    /// </summary>
    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;
        private readonly IDeploymentService _deploymentService;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public HomeController()
        {
            _environmentRepository = Bootstrapper.Resolve<IEnvironmentRepository>();
            _deploymentService = Bootstrapper.Resolve<IDeploymentService>();
            _pipelineRepository = Bootstrapper.Resolve<IPipelineRepository>();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Deploy(int pipelineId)
        {
            const int PageSize = 30;
            IEnumerable<Environment> environments =
                _environmentRepository.GetEnvironments(new PagingInfo(PageSize));
            Pipeline pipeline = _pipelineRepository.GetPipeline(pipelineId);
            IEnumerable<Deployment> deployments = _deploymentService.GetDeployments(pipelineId);
            IEnumerable<EnvironmentViewModel> environmentViewModels = environments.Select(
                e => new EnvironmentViewModel
                         {
                             EnvironmentId = e.Id,
                             EnvironmentName = e.EnvironmentName
                         });
            PipelineViewModel pipelineViewModel = PipelineMapper.MapToPipelineViewModel(pipeline);
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
            _deploymentService.ScheduleDeployment(pipelineId, environmentId);
            return RedirectToAction("Deploy", new { PipelineId = pipelineId });
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Pipeline> pipelines = _pipelineRepository.GetPipelines(BuildPageSize);
            PipelineIndexViewModel viewModel = PipelineMapper.MapToPipelineIndexViewModel(pipelines);
            return View(viewModel);
        }
    }
}