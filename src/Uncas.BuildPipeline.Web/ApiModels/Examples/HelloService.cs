using ServiceStack.ServiceHost;

namespace Uncas.BuildPipeline.Web.ApiModels.Examples
{
    public class HelloService : IService
    {
        public object Any(Hello request)
        {
            return new HelloResponse {Result = "Hello, *** " + request.Name};
        }
    }
}