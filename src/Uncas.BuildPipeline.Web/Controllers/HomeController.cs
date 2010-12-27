namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var builds = new Repository().GetBuilds(10);
            var viewModels = builds.Select(b => new BuildViewModel
            {
                ProjectName = b.ProjectName,
                SourceRevision = b.SourceRevision
            });
            return View(viewModels);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
