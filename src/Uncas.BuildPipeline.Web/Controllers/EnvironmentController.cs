namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class EnvironmentController : BaseController
    {
        private readonly IDeploymentService deploymentService;
        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;

        public EnvironmentController()
        {
            this.environmentRepository = Bootstrapper.GetEnvironmentRepository();
            this.deploymentService = Bootstrapper.GetDeploymentService();
            this.pipelineRepository = Bootstrapper.GetPipelineRepository();
        }

        public ActionResult Index()
        {
            var environments = this.environmentRepository.GetEnvironments();
            var viewModel = new List<EnvironmentIndexViewModel>();
            foreach (var environment in environments)
            {
                AddEnvironmentIndexViewModel(viewModel, environment);
            }

            return View(viewModel);
        }

        public void AddEnvironmentIndexViewModel(
            IList<EnvironmentIndexViewModel> viewModels,
            Environment environment)
        {
            var deployments = this.deploymentService.GetDeploymentsByEnvironment(environment.Id);
            var lastDeployment = deployments.Where(d => d.Completed.HasValue).OrderByDescending(d => d.Completed).FirstOrDefault();
            if (lastDeployment == null)
                return;
            var pipeline = this.pipelineRepository.GetPipeline(lastDeployment.PipelineId);
            int currentSourceRevision = pipeline.SourceRevision;
            viewModels.Add(new EnvironmentIndexViewModel
                {
                    EnvironmentId = environment.Id,
                    EnvironmentName = environment.EnvironmentName,
                    CurrentSourceRevision = currentSourceRevision
                });
        }
    }
}