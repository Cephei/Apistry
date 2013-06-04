namespace Apistry.Samples.Service.Api.Registry
{
    using System.Web.Http;
    using NContext.Extensions.AspNetWebApi.Routing;

    public class BlogsRouteRegistry : IConfigureHttpRouting
    {
        public void Configure(IManageHttpRouting routingManager)
        {
            routingManager.RegisterHttpRoute(
                routeName: "Blogs",
                routeTemplate: "api/blogs/{blogId}",
                defaults: new { controller = "Blogs", blogId = RouteParameter.Optional });
        }

        public int Priority
        {
            get { return 40; }
        }
    }
}