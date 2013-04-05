using System;
using Funq;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using Uncas.BuildPipeline.Web.App_Start;
using Uncas.BuildPipeline.Web.Configuration;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AppHost), "Start")]

namespace Uncas.BuildPipeline.Web.App_Start
{
    //A customizeable typed UserSession that can be extended with your own properties
    //To access ServiceStack's Session, Cache, etc from MVC Controllers inherit from ControllerBase<CustomUserSession>
    [CLSCompliant(false)]
    public class CustomUserSession : AuthUserSession
    {
        public string CustomProperty { get; set; }
    }

    [CLSCompliant(false)]
    public class AppHost : AppHostBase
    {
        public AppHost() : base("BuildPipeline ASP.NET Host", typeof (AppHost).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;
            container.Adapter = new UnityContainerAdapter();
        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}