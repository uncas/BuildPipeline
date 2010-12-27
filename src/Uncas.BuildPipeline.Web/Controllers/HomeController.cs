namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Web.Mvc;
    using Uncas.BuildPipeline.Web.Mappers;
    using Uncas.BuildPipeline.Web.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var builds = new Repository().GetBuilds(10);
            var viewModels = BuildMapper.MapToBuildViewModels(builds);
            return View(viewModels);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
