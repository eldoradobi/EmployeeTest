using MetinvestTest.DAL;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Mvc = Unity.Mvc5;
using Api = Unity.WebApi;

namespace MetinvestTest
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<AppDbContext>();
            GlobalConfiguration.Configuration.DependencyResolver = new Api.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new Mvc.UnityDependencyResolver(container));
        }
    }
}