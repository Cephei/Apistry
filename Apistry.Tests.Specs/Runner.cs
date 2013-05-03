
namespace Apistry.Tests.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using Machine.Specifications;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class Runner
    {
        Establish ctx = () =>
        {
            config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/example/5?role=admin")
            {
                Content = new StringContent("{ \"Name\": \"DGDev\" }")
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var route = config.Routes.MapHttpRoute("Example", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Example" } });
            _Controller = new ExampleController
                {
                    Configuration = config,
                    ControllerContext = new HttpControllerContext(config, routeData, request)
                        {
                            ControllerDescriptor = new HttpControllerDescriptor(config, "ExampleController", typeof(ExampleController))
                        },
                    Request = request
                };

            _Controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        };

        Because of = () =>
        {
            
        };

        It should_run_the_test = () =>
        {
            WebApiDocumentationMetadata metadata =
                new WebApiDocumentationMetadataBuilder()
                    .DocumentDto<User>("System user.")
                        .For(u => u.Id)
                            .Description("A unique identifier for a user used by the application.")
                            .Example(53)
                            .IsRequired(HttpMethod.Put)
                        .For(u => u.Name)
                            .Description("A unique name to identify a user.")
                            .Example("dgdev")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(u => u.Address)
                            .IsRequired(HttpMethod.Post)
                    .DocumentDto<Address>("A user's address.")
                        .For(a => a.StreetNumber)
                            .Description("The street number.")
                            .Example(150)
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(a => a.Street)
                            .Description("The street.")
                            .Example("E Robinson St.")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(a => a.City)
                            .Description("The city name.")
                            .Example("Orlando")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(a => a.State)
                            .Description("The state.")
                            .Example("Florida")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(a => a.ZipCode)
                            .Description("The zip code.")
                            .Example(32801)
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(a => a.Country)
                            .Description("The country.")
                            .Example("United States")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                    .DocumentDto<Profile>("A user's profile.")
                        .For(p => p.About)
                            .Description("About user profile section.")
                            .Example("A developer originally from South Florida.")
                        .For(p => p.Picture)
                            .Description("The profile picture.")

                    .DocumentController<ExampleController>()
                        .Resource("Users")
                        .Summary("User-related API operations.")
                        .DescribeAction(c => c.CreateUser(default(Int32), default(User), default(String)))
                            .Name("Create a new user account.")
                            .Summary(@"This is will create a new user with associated profile information.")
                            .DescribeParameter("siteId", typeof(Int32), "The tenant identifier.")
                            .DescribeParameter("user", typeof(User))
                            .DescribeParameter("role", typeof(String), "The membership role.")
                            .Returns<User>(HttpStatusCode.Created)
                            .Alert("This endpoint is only accessible by administrators.")
                            .Information("Important information regarding this endpoint.");

            var provider = new WebApiDocumentationProvider(metadata);

            var documentation = provider.GetDocumentation(
                new ReflectedHttpActionDescriptor(
                    _Controller.ControllerContext.ControllerDescriptor,
                    _Controller.ControllerContext.ControllerDescriptor.ControllerType.GetMethod("CreateUser", BindingFlags.Instance | BindingFlags.Public)));

            var docs =
                JsonSerializer.Create(new JsonSerializerSettings())
                              .Deserialize<HttpActionDocumentation>(
                                  new JsonTextReader(new StringReader(documentation)));

            var pDoc = provider.GetDocumentation(
                new ReflectedHttpParameterDescriptor(
                    new ReflectedHttpActionDescriptor(
                        new HttpControllerDescriptor(config, "CreateUser", typeof(ExampleController)), 
                        typeof(ExampleController).GetMethod("CreateUser", BindingFlags.Instance | BindingFlags.Public))
                    , typeof(ExampleController).GetMethod("CreateUser", BindingFlags.Instance | BindingFlags.Public).GetParameters()[0]));
        };
        
        private static ExampleController _Controller;
        private static HttpConfiguration config;
    }

    public class User
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public Address Address { get; set; }

        public Profile Profile { get; set; }

        public IEnumerable<User> Friends { get; set; }
    }

    public class Profile
    {
        public Byte[] Picture { get; set; }

        public String About { get; set; }
    }

    public class Address
    {
        public Int32 StreetNumber { get; set; }

        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public Int32 ZipCode { get; set; }

        public String Country { get; set; }
    }
}
