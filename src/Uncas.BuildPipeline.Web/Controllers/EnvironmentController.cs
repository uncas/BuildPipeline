using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.ViewModels;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Web.Controllers
{
    /// <summary>
    /// Handles environments.
    /// </summary>
    public class EnvironmentController : BaseController
    {
        private readonly IDeploymentRepository _deploymentRepository;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public EnvironmentController(
            IDeploymentRepository deploymentRepository,
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository)
        {
            _deploymentRepository = deploymentRepository;
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            const int pageSize = 30;
            IEnumerable<Environment> environments =
                _environmentRepository.GetEnvironments(new PagingInfo(pageSize));
            var viewModel = new List<EnvironmentIndexViewModel>();
            foreach (Environment environment in environments)
                AddEnvironmentIndexViewModel(viewModel, environment);

            return View(viewModel);
        }

        private void AddEnvironmentIndexViewModel(
            IList<EnvironmentIndexViewModel> viewModels,
            Environment environment)
        {
            if (environment == null)
                throw new ArgumentNullException("environment");

            if (viewModels == null)
                throw new ArgumentNullException("viewModels");

            IEnumerable<Deployment> deployments =
                _deploymentRepository.GetByEnvironment(
                    environment.Id);
            Deployment lastDeployment =
                deployments.Where(d => d.Completed.HasValue).OrderByDescending(
                    d => d.Completed).FirstOrDefault();
            if (lastDeployment == null)
                return;

            Pipeline pipeline = _pipelineRepository.GetPipeline(lastDeployment.PipelineId);
            string currentRevision = pipeline.Revision;
            viewModels.Add(new EnvironmentIndexViewModel
                {
                    EnvironmentId = environment.Id,
                    EnvironmentName = environment.EnvironmentName,
                    CurrentRevision = currentRevision
                });
        }
    }
}