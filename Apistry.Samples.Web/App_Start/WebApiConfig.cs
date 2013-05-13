

namespace Apistry.Samples.Web
{
    using System.Web.Http;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Description;
    using Apistry.Samples.Web.Controllers;
    using Apistry.Samples.Web.Models;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            WebApiDocumentationMetadata metadata =
                new WebApiDocumentationMetadataBuilder()
                    .DocumentDto<User>("System user.")
                        .For(u => u.Id)
                            .Description("A unique identifier for a user used by the application.")
                            .Example(53)
                        .For(u => u.FirstName)
                            .Description("A user's first name.")
                            .Example("Daniel")
                        .For(u => u.LastName)
                            .Description("A user's last name.")
                            .Example("Gioulakis")
                    .DocumentDto<Address>("A user's address.")
                        .For(a => a.AddressLine1)
                            .Description("The street number.")
                            .Example("150 E Robinson St")
                        .For(a => a.City)
                            .Description("The city name.")
                            .Example("Orlando")
                        .For(a => a.State)
                            .Description("The state.")
                            .Example("Florida")
                        .For(a => a.ZipCode)
                            .Description("The zip code.")
                            .Example("32801")
                        .For(a => a.Country)
                            .Description("The country.")
                            .Example("United States")
                    .DocumentDto<Profile>("A user's profile.")
                        .For(p => p.About)
                            .Description("About user profile section.")
                            .Example("A developer originally from South Florida.")
                        .For(p => p.Picture)
                            .Description("The profile picture.")

                    .DocumentController<UserController>()
                        .Resource("Users")
                        .Summary("User-related API operations.")
                        .DescribeAction(c => c.Post(default(User)))
                            .Name("Create a new user account.")
                            .Summary(@"This is will create a new user with associated profile information.")
                            .DescribeParameter<User>("user")
                            .Returns(HttpStatusCode.Created)
                            .Alert("This endpoint is only accessible by administrators.")
                            .Information("Important information regarding this endpoint.");

            config.Services.Replace(typeof(IDocumentationProvider), new WebApiDocumentationProvider(metadata));
        }
    }
}