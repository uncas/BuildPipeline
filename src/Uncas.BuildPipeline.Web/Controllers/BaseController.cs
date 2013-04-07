using System.Web.Mvc;

namespace Uncas.BuildPipeline.Web.Controllers
{
    /// <summary>
    /// Base controller for the web site.
    /// </summary>
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewData["Version"] = ApplicationVersion.GetVersion(GetType().Assembly);
        }
    }
}