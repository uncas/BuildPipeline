namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Base controller for the web site.
    /// </summary>
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewData["Version"] = 
                this.GetType().Assembly.GetName().Version.ToString();
        }
    }
}