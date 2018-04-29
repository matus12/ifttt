using Contracts;
using Contracts.Services;
using Contracts.Wrappers;
using Unity;
using Unity.Lifetime;

namespace ContentService
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public IUnityContainer RegisterTypes(IUnityContainer container)
            => container
                .RegisterType<IHttpClientWrapper, Wrappers.HttpClientWrapper>(new ContainerControlledLifetimeManager())
                .RegisterType<IContentService, Services.ContentService>(new ContainerControlledLifetimeManager());
    }
}