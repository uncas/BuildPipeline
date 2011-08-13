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

    public class EnvironmentController : BaseController
    {
        private readonly IDeploymentService deploymentService;
        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;

        public EnvironmentController()
        {
            environmentRepository = Bootstrapper.Resolve<IEnvironmentRepository>();
            deploymentService = Bootstrapper.Resolve<IDeploymentService>();
            pipelineRepository = Bootstrapper.Resolve<IPipelineRepository>();
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
                deploymentService.GetDeploymentsByEnvironment(
                    environment.Id);
            Deployment lastDeployment =
                deployments.Where(d => d.Completed.HasValue).OrderByDescending(d => d.Completed).FirstOrDefault();
            if (lastDeployment == null)
            {
                return;
            }

            Pipeline pipeline = pipelineRepository.GetPipeline(lastDeployment.PipelineId);
            int currentSourceRevision = pipeline.SourceRevision;
            viewModels.Add(new EnvironmentIndexViewModel
                               {
                                   EnvironmentId = environment.Id,
                                   EnvironmentName = environment.EnvironmentName,
                                   CurrentSourceRevision = currentSourceRevision
                               });
        }

        public ActionResult Index()
        {
            const int pageSize = 30;
            IEnumerable<Environment> environments =
                environmentRepository.GetEnvironments(new PagingInfo(pageSize));
            var viewModel = new List<EnvironmentIndexViewModel>();
            foreach (Environment environment in environments)
            {
                AddEnvironmentIndexViewModel(viewModel, environment);
            }

            return View(viewModel);
        }
    }
}