namespace Uncas.BuildPipeline.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.Core;

    /// <summary>
    /// Setup logic for the website.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Mvc",
        Justification = "Name of framework.")]
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "Standard handler in ASP.NET MVC.")]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            BuildPipelineDatabaseSetup.Setup(
                ConfigurationWrapper.GetConnectionString(
                    "BuildPipelineConnectionString"));
        }
    }
}