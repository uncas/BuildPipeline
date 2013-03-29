using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Uncas.BuildPipeline.Web.Configuration;

namespace Uncas.BuildPipeline.Web
{
    /// <summary>
    /// Setup logic for the website.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Mvc", Justification = "Name of framework.")]
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("api/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default",
                            "{controller}/{action}/{id}",
                            new
                                {
                                    controller = "Home",
                                    action = "Index",
                                    id = UrlParameter.Optional
                                });
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Standard handler in ASP.NET MVC.")]
        protected void Application_Start()
        {
            WebBootstrapper.InitialiseForWeb();
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            WebBootstrapper.Resolve<ILogger>().Info("Starting website.");
        }

        public override void Init()
        {
            Error += OnError;
            base.Init();
        }

        private void OnError(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            WebBootstrapper.Resolve<ILogger>().Error(lastError, "Unhandled exception occurred.");
        }
    }
}