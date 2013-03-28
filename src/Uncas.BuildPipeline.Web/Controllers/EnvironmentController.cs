namespace Uncas.BuildPipeline.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Web.ViewModels;
    using Uncas.Core.Data;
    using Environment = Uncas.BuildPipeline.Models.Environment;

    /// <summary>
    /// Handles environments.
    /// </summary>
    public class EnvironmentController : BaseController
    {
        private readonly IDeploymentService _deploymentService;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public EnvironmentController(
            IDeploymentService deploymentService,
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository)
        {
            _deploymentService = deploymentService;
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
        }

        public void AddEnvironmentIndexViewModel(
            IList<EnvironmentIndexViewModel> viewModels,
            Environment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            if (viewModels == null)
            {
                throw new ArgumentNullException("viewModels");
            }

            IEnumerable<Deployment> deployments =
                _deploymentService.GetDeploymentsByEnvironment(
                    environment.Id);
            Deployment lastDeployment =
                deployments.Where(d => d.Completed.HasValue).OrderByDescending(d => d.Completed).FirstOrDefault();
            if (lastDeployment == null)
            {
                return;
            }

            Pipeline pipeline = _pipelineRepository.GetPipeline(lastDeployment.PipelineId);
            string currentRevision = pipeline.Revision;
            viewModels.Add(new EnvironmentIndexViewModel
                               {
                                   EnvironmentId = environment.Id,
                                   EnvironmentName = environment.EnvironmentName,
                                   CurrentRevision = currentRevision
                               });
        }

        public ActionResult Index()
        {
            const int PageSize = 30;
            IEnumerable<Environment> environments =
                _environmentRepository.GetEnvironments(new PagingInfo(PageSize));
            var viewModel = new List<EnvironmentIndexViewModel>();
            foreach (Environment environment in environments)
            {
                AddEnvironmentIndexViewModel(viewModel, environment);
            }

            return View(viewModel);
        }
    }
}