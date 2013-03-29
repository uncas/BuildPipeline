using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.Mappers;
using Uncas.BuildPipeline.Web.ViewModels;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Web.Controllers
{
    /// <summary>
    /// Handles the front page.
    /// </summary>
    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        private readonly IDeploymentService _deploymentService;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly ILogger _logger;
        private readonly IPipelineRepository _pipelineRepository;

        public HomeController(
            IDeploymentService deploymentService,
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository,
            ILogger logger)
        {
            _deploymentService = deploymentService;
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Pipeline> pipelines = _pipelineRepository.GetPipelines(BuildPageSize);
            PipelineIndexViewModel viewModel = PipelineMapper.MapToPipelineIndexViewModel(pipelines);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Deploy(int pipelineId)
        {
            const int pageSize = 30;
            IEnumerable<Environment> environments =
                _environmentRepository.GetEnvironments(new PagingInfo(pageSize));
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
                                    e => e.Id == d.EnvironmentId).Maybe(x => x.EnvironmentName)
                            }),
                    Pipeline = pipelineViewModel
                };
            return View(deployViewModel);
        }

        [HttpPost]
        public ActionResult Deploy(int environmentId, int pipelineId)
        {
            _deploymentService.ScheduleDeployment(pipelineId, environmentId);
            return RedirectToAction("Deploy", new {PipelineId = pipelineId});
        }

        [HttpGet]
        public ActionResult Download(string id)
        {
            string filePath = Path.Combine(CustomApiController.PackageFolder, id);
            if (!System.IO.File.Exists(filePath))
                return HttpNotFound("File not found.");
            return new FilePathResult(filePath, "application/zip");
        }
    }
}