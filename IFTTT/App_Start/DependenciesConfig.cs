using System.Web.Http;
using ContentService;
using Contracts;
using IFTTT.Resolver;
using Unity;

namespace IFTTT
{
    internal static class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var apiDependencyContainer = new UnityContainer()
                .Register<ServicesBootstrapper>();
            config.DependencyResolver = new UnityResolver(apiDependencyContainer);
        }

        private static IUnityContainer Register<TBootstrapper>(this IUnityContainer container)
            where TBootstrapper : IBootstrapper, new()
            => new TBootstrapper().RegisterTypes(container);
    }
}