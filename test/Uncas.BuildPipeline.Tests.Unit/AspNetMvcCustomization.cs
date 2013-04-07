using System.Web;
using Moq;
using Ploeh.AutoFixture;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public class AspNetMvcCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            SetupContext(fixture);
        }

        #endregion

        private static void SetupContext(IFixture fixture)
        {
            var httpContext = fixture.Freeze<Mock<HttpContextBase>>();
            Mock<HttpRequestBase> requestMock = GetRequestMock(fixture);
            Mock<HttpResponseBase> responseMock = GetResponseMock(fixture);
            httpContext.SetupGet(x => x.Request).Returns(requestMock.Object);
            httpContext.SetupGet(x => x.Response).Returns(responseMock.Object);
        }

        private static Mock<HttpResponseBase> GetResponseMock(IFixture fixture)
        {
            var responseMock = fixture.Freeze<Mock<HttpResponseBase>>();
            return responseMock;
        }

        private static Mock<HttpRequestBase> GetRequestMock(IFixture fixture)
        {
            var requestMock = fixture.Freeze<Mock<HttpRequestBase>>();
            requestMock.SetupGet(x => x.Files).Returns(GetFilesMock(fixture).Object);
            return requestMock;
        }

        private static Mock<HttpFileCollectionBase> GetFilesMock(IFixture fixture)
        {
            var filesMock = fixture.Freeze<Mock<HttpFileCollectionBase>>();
            filesMock.SetupGet(x => x.Count).Returns(1);
            Mock<HttpPostedFileBase> fileMock = fixture.FreezeMock<HttpPostedFileBase>();
            fileMock.SetupGet(x => x.FileName).Returns(fixture.Create<string>());
            fileMock.Setup(x => x.SaveAs(It.IsAny<string>()));
            filesMock.SetupGet(x => x[0]).Returns(fileMock.Object);
            return filesMock;
        }
    }
}