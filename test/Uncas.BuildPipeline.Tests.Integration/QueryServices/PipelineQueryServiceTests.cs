using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.QueryServices;
using Uncas.BuildPipeline.Web.ViewModels;

namespace Uncas.BuildPipeline.Tests.Integration.QueryServices
{
    public class PipelineQueryServiceTests : WithBootstrapping<PipelineQueryService>
    {
        [Test]
        public void GetPipelines_Existing_GetsAtLeastOne()
        {
            Resolve<IPipelineRepository>().AddPipeline(Fixture.Create<Pipeline>());

            IEnumerable<PipelineListItemViewModel> pipelines = Sut.GetPipelines(1);

            CollectionAssert.IsNotEmpty(pipelines);
        }
    }
}