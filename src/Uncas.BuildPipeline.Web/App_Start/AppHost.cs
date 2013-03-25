using System.Web.Mvc;
using Funq;
using ServiceStack.Mvc;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.ApiModels.Examples;
using Uncas.BuildPipeline.Web.App_Start;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AppHost), "Start")]

namespace Uncas.BuildPipeline.Web.App_Start
{
    //A customizeable typed UserSession that can be extended with your own properties
    //To access ServiceStack's Session, Cache, etc from MVC Controllers inherit from ControllerBase<CustomUserSession>
    public class CustomUserSession : AuthUserSession
    {
        public string CustomProperty { get; set; }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("BuildPipeline ASP.NET Host", typeof (AppHost).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;

            //Configure User Defined REST Paths
            Routes.Add<Hello>("/hello").Add<Hello>("/hello/{Name*}");

            //Register all your dependencies
            container.Register(new TodoRepository());
            container.Register(Bootstrapper.Resolve<IPipelineRepository>());

            //Set MVC to use the same Funq IOC as ServiceStack
            ControllerBuilder.Current.SetControllerFactory(
                new FunqControllerFactory(container));
        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}