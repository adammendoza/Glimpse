﻿using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.PipelineInspector;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.PipelineInspector
{
    public class ModelBinderInspectorShould
    {
        [Fact]
        public void Constuct()
        {
            var sut = new ModelBinderInspector();

            Assert.IsAssignableFrom<IPipelineInspector>(sut);
        }

        [Theory, AutoMock]
        public void IgnoreEmptyModelBindingProvidersCollection(ModelBinderInspector sut, IPipelineInspectorContext context, IModelBinderProvider proxy)
        {
            ModelBinderProviders.BinderProviders.Clear();
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<IModelBinderProvider>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<IModelBinderProvider>(), It.IsAny<IEnumerable<IAlternateImplementation<IModelBinderProvider>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Empty(ModelBinderProviders.BinderProviders);
        }

        [Theory, AutoMock]
        public void UpdateModelBindingProviders(ModelBinderInspector sut, IPipelineInspectorContext context, IModelBinderProvider proxy)
        {
            ModelBinderProviders.BinderProviders.Add(new DummyModelBinderProvider());
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<IModelBinderProvider>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<IModelBinderProvider>(), It.IsAny<IEnumerable<IAlternateImplementation<IModelBinderProvider>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ModelBinderProviders.BinderProviders);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void IgnoreEmptyValueProviderFactoriesCollection(ModelBinderInspector sut, IPipelineInspectorContext context, ValueProviderFactory proxy)
        {
            ValueProviderFactories.Factories.Clear();
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<ValueProviderFactory>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<ValueProviderFactory>(), It.IsAny<IEnumerable<IAlternateImplementation<ValueProviderFactory>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Empty(ValueProviderFactories.Factories);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never());
        }

        [Theory, AutoMock]
        public void UpdateValueProviderFactories(ModelBinderInspector sut, IPipelineInspectorContext context, ValueProviderFactory proxy)
        {
            ValueProviderFactories.Factories.Add(new DummyValueProviderFactory());
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<ValueProviderFactory>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<ValueProviderFactory>(), It.IsAny<IEnumerable<IAlternateImplementation<ValueProviderFactory>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ValueProviderFactories.Factories);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void UpdateModelBinders(ModelBinderInspector sut, IPipelineInspectorContext context, DummyDefaultModelBinder seedBinder, DefaultModelBinder proxy)
        {
            ModelBinders.Binders.Add(typeof(object), seedBinder);
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<DefaultModelBinder>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<DefaultModelBinder>(), It.IsAny<IEnumerable<IAlternateImplementation<DefaultModelBinder>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ModelBinders.Binders.Values);
            Assert.DoesNotContain(seedBinder, ModelBinders.Binders.Values);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void UpdateDefaultModelBinder(ModelBinderInspector sut, IPipelineInspectorContext context, DefaultModelBinder proxy)
        {
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<DefaultModelBinder>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<DefaultModelBinder>(), It.IsAny<IEnumerable<IAlternateImplementation<DefaultModelBinder>>>(), null, null)).Returns(proxy);

            sut.Setup(context);

            Assert.Equal(proxy, ModelBinders.Binders.DefaultBinder);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }
    }
}