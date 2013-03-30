using System;
using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public class TestObjectFactory : IFixture
    {
        private readonly IFixture _fixture;

        private TestObjectFactory()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        #region IFixture Members

        public ICustomizationComposer<T> Build<T>()
        {
            return _fixture.Build<T>();
        }

        public IFixture Customize(ICustomization customization)
        {
            return _fixture.Customize(customization);
        }

        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            _fixture.Customize(composerTransformation);
        }

        public IList<ISpecimenBuilderTransformation> Behaviors
        {
            get { return _fixture.Behaviors; }
        }

        public IList<ISpecimenBuilder> Customizations
        {
            get { return _fixture.Customizations; }
        }

        public bool OmitAutoProperties
        {
            get { return _fixture.OmitAutoProperties; }
            set { _fixture.OmitAutoProperties = value; }
        }

        public int RepeatCount
        {
            get { return _fixture.RepeatCount; }
            set { _fixture.RepeatCount = value; }
        }

        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return _fixture.ResidueCollectors; }
        }

        public object Create(object request, ISpecimenContext context)
        {
            return _fixture.Create(request, context);
        }

        #endregion

        public static TestObjectFactory SetupFixture()
        {
            return new TestObjectFactory();
        }
    }
}