using System;
using System.Net;
using ServiceStack.ServiceInterface;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Web.ApiModels
{
    [CLSCompliant(false)]
    public class PipelineService : Service
    {
        public IPipelineRepository Repository { get; set; }

        public object Get(PipelineRequest request)
        {
            Pipeline pipeline = Repository.GetPipeline(request.PipelineId);
            if (pipeline == null)
            {
                base.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return new {ErrorMessage = "The pipeline was not found."};
            }

            return pipeline;
        }

        public object Post(PipelineRequest request)
        {
            var pipeline = new Pipeline(0,
                                        request.ProjectName,
                                        request.Revision,
                                        request.BranchName,
                                        request.BranchName,
                                        DateTime.Now,
                                        "NN",
                                        request.PackagePath);
            Repository.AddPipeline(pipeline);
            return pipeline.PipelineId;
        }
    }
}