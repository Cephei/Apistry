namespace Apistry.Samples.Presentation.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Apistry.Samples.Presentation.Web.App_Start;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Configuration;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationConfiguration appConfig =
                new ApplicationConfigurationBuilder()
                    .ComposeForWeb(fileName => fileName.StartsWith("Apistry.Samples"))
                    .RegisterComponent<IManageWebApi>()
                        .With<WebApiManagerBuilder>()
                            .ConfigureForIIS();

            Configure.Using(appConfig);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}