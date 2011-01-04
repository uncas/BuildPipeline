namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class EnvironmentController : BaseController
    {
        private readonly IEnvironmentRepository environmentRepository;

        public EnvironmentController()
        {
            this.environmentRepository = Bootstrapper.GetEnvironmentRepository();
        }

        public ActionResult Index()
        {
            var environments = this.environmentRepository.GetEnvironments();
            // TODO: Feature: Retrieve current source revision for the environment:
            var viewModel = environments.Select(e =>
                new EnvironmentIndexViewModel
                {
                    EnvironmentId = e.Id,
                    EnvironmentName = e.EnvironmentName,
                    CurrentSourceRevision = 123
                });
            return View(viewModel);
        }
    }
}