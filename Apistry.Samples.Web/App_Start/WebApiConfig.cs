

namespace Apistry.Samples.Web
{
    using System.Collections.Generic;
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
                name: "Users",
                routeTemplate: "api/users/{userId}",
                defaults: new { controller = "Users", userId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Blogs",
                routeTemplate: "api/blogs/{blogId}",
                defaults: new { controller = "Blogs", blogId = RouteParameter.Optional }
            );

            WebApiDocumentationMetadata metadata =
                new WebApiDocumentationMetadataBuilder()
                    .DocumentDto<UserDto>("System user")
                        .For(u => u.Id)
                            .Description("A unique identifier for a user used by the application.")
                            .Example(53)
                        .For(u => u.FirstName)
                            .Description("A user's first name.")
                            .Example("Daniel")
                        .For(u => u.LastName)
                            .Description("A user's last name.")
                            .Example("Gioulakis")
                        .For(u => u.Address)
                        .For(u => u.Profile)
                        .For(u => u.Friends)
                    .DocumentDto<AddressDto>("A user's address.")
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
                    .DocumentDto<ProfileDto>("A user's profile.")
                        .For(p => p.About)
                            .Description("About user profile section.")
                            .Example("A developer originally from South Florida.")
                        .For(p => p.Picture)
                            .Description("The profile picture.")

                    .DocumentController<UsersController>()
                        .Resource("Users")
                        .Summary("User-related API operations.")
                        .DescribeAction(c => c.DeleteUser(default(int)))
                            .Name("Delete a User")
                            .Summary("Delete a user and all associated data.")
                            .DescribeParameter<int>("userId", "The user's identity.")
                            .Returns(HttpStatusCode.NoContent)
                        .DescribeAction(c => c.GetUsers())
                            .Name("Get all Users")
                            .Summary("Returns all user's for your site.")
                            .Returns<IEnumerable<UserDto>>()
                        .DescribeAction(c => c.GetUser(default(int)))
                            .Name("Get a User")
                            .Summary("Gets a specific user.")
                            .Returns<UserDto>()
                        .DescribeAction(c => c.PatchUser(default(int), default(PatchRequest<UserDto>)))
                            .Name("Modify a User")
                            .Summary("Modify specific attributes about a user.")
                            .DescribeParameter<int>("userId", "The user's identity.")
                            .DescribeParameter<PatchRequest<UserDto>>("patchRequest", "The key-value user properties to patch.")
                            .Returns(HttpStatusCode.NoContent)
                        .DescribeAction(c => c.PostUser(default(UserDto)))
                            .Name("Create a new user account.")
                            .Summary(@"This is will create a new user with associated profile information.")
                            .DescribeParameter<UserDto>("userDto")
                            .Returns(HttpStatusCode.Created)
                            .Alert("This endpoint is only accessible by administrators.")
                            .Information("Important information regarding this endpoint.")
                        .DescribeAction(c => c.PutUser(default(int), default(UserDto)))
                            .Name("Update a User")
                            .Summary("Update the entire user entity.")
                            .Returns(HttpStatusCode.NoContent)
                            ;

            config.Services.Replace(typeof(IDocumentationProvider), new WebApiDocumentationProvider(metadata));
        }
    }
}