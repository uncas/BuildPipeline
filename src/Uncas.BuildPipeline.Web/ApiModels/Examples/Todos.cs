using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace Uncas.BuildPipeline.Web.ApiModels.Examples
{
    [Route("/todos")]
    [Route("/todos/{Ids}")]
    public class Todos : IReturn<List<Todo>>
    {
        public Todos(params long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }
}