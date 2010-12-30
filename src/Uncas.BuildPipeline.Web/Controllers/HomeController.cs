namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class HomeController : BaseController
    {
        private const int BuildPageSize = 10;

        private readonly Repository pipelineRepository;

        public HomeController()
        {
            this.pipelineRepository = new Repository();
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
            var viewModel = new List<EnvironmentViewModel>();
            // TODO: Fetch these data from repository:
            viewModel.Add(new EnvironmentViewModel
            {
                EnvironmentId = 1,
                EnvironmentName = "Integration"
            });
            viewModel.Add(new EnvironmentViewModel
            {
                EnvironmentId = 2,
                EnvironmentName = "QA"
            });
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Deploy(int environmentId, int pipelineId)
        {
            // TODO: Put this in application service:
            var pipeline = this.pipelineRepository.GetPipeline(pipelineId);
            string packagePath = pipeline.PackagePath;
            // TODO: Give proper feed back to user...
            var deploymentUtility = new DeploymentUtility();
            deploymentUtility.Deploy(packagePath, @"C:\temp\deploytest");
            return RedirectToAction("Deploy", new { PipelineId = pipelineId });
        }

        private void WriteEnvironmentProperties(int environmentId)
        {
            string websiteDestinationPath =
                @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web";
            string websiteName = "BuildPipelineWeb";
            int websitePort = 876;
            if (environmentId == 2)
            {
                websiteDestinationPath =
                    @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web.QA";
                websiteName = "BuildPipelineWeb-QA";
                websitePort = 872;
            }

            string format = @"
<?xml version=""1.0""?>
<properties>

  <property name=""website.destination.path"" value=""{0}"" />
  <property name=""website.name"" value=""{1}"" />
  <property name=""website.port"" value=""{2}"" />

</properties>
";
            string properties = string.Format(
                format,
                websiteDestinationPath,
                websiteName,
                websitePort);
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
