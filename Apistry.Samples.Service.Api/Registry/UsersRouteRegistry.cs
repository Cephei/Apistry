namespace Apistry.Samples.Service.Api.Registry
{
    using System.Web.Http;
    using NContext.Extensions.AspNetWebApi.Routing;

    public class UsersRouteRegistry : IConfigureHttpRouting
    {
        public void Configure(IManageHttpRouting routingManager)
        {
            routingManager.RegisterHttpRoute(
                routeName: "Users",
                routeTemplate: "api/users/{userId}",
                defaults: new { controller = "Users", userId = RouteParameter.Optional });
        }

        public int Priority
        {
            get { return 40; }
        }
    }
}