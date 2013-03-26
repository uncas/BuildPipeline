using System;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Uncas.BuildPipeline.Web.ApiModels.Examples
{
    public class HelloResponse
    {
        public string Result { get; set; }

        [CLSCompliant(false)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}