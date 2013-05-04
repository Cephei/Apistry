
namespace Apistry.Tests.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
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
                Content = new StringContent("{ \"FirstName\": \"DGDev\" }")
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
                        .For(u => u.FirstName)
                            .Description("A user's first name.")
                            .Example("Daniel")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(u => u.LastName)
                            .Description("A user's last name.")
                            .Example("Gioulakis")
                            .IsRequired(HttpMethod.Post, HttpMethod.Put)
                        .For(u => u.Address)
                            .IsRequired(HttpMethod.Post)
                    .DocumentDto<Address>("A user's address.")
                        .For(a => a.AddressLine1)
                            .Description("The street number.")
                            .Example("150 E Robinson St")
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
                            .Example("32801")
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
                        .DescribeAction(c => c.PostUser(default(Int32), default(User), default(String)))
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
                    _Controller.ControllerContext.ControllerDescriptor.ControllerType.GetMethod("PostUser", BindingFlags.Instance | BindingFlags.Public)));

            var docs =
                JsonSerializer.Create(new JsonSerializerSettings())
                              .Deserialize<HttpActionDocumentation>(
                                  new JsonTextReader(new StringReader(documentation)));

            var pDoc = provider.GetDocumentation(
                new ReflectedHttpParameterDescriptor(
                    new ReflectedHttpActionDescriptor(
                        new HttpControllerDescriptor(config, "PostUser", typeof(ExampleController)),
                        typeof(ExampleController).GetMethod("PostUser", BindingFlags.Instance | BindingFlags.Public))
                    , typeof(ExampleController).GetMethod("PostUser", BindingFlags.Instance | BindingFlags.Public).GetParameters()[0]));
        };
        
        private static ExampleController _Controller;
        private static HttpConfiguration config;
    }
    
    /// <summary>
    /// Defines a data-transfer object contract used for functional composition.
    /// </summary>
    /// <typeparam name="T">Type of data to return.</typeparam>
    /// <remarks></remarks>
    public interface IResponseTransferObject<T> : IDisposable
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <remarks></remarks>
        T Data { get; }
        
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<Error> Errors { get; }
    }

    public class ServiceResponse<T> : IResponseTransferObject<T>
    {
        private T _Data;

        private IEnumerable<Error> _Errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public ServiceResponse(T data)
            : this(data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        public ServiceResponse(Error error)
            : this(new List<Error> { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="errors">The response errors.</param>
        /// <remarks></remarks>
        public ServiceResponse(IEnumerable<Error> errors)
            : this(default(T), errors)
        {
        }

        /// <summary>
        /// For deserialization purposes only.
        /// </summary>
        /// <remarks></remarks>
        protected ServiceResponse()
        {
            Data = default(T);
            Errors = Enumerable.Empty<Error>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IResponseTransferObject{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        private ServiceResponse(T data, IEnumerable<Error> errors)
        {
            Data = (data != null) ? data : default(T);
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        /// <summary>
        /// Gets an empty <see cref="ServiceResponse{T}"/>.
        /// </summary>
        /// <remarks></remarks>
        public static IResponseTransferObject<T> Empty
        {
            get
            {
                return new ServiceResponse<T>(default(T));
            }
        }

        /// <summary>
        /// Gets or sets the <typeparam name="T"/> data.
        /// </summary>
        public T Data
        {
            get
            {
                return _Data;
            }
            protected set
            {
                _Data = value;
            }
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Error> Errors
        {
            get
            {
                return _Errors;
            }
            protected set
            {
                _Errors = value;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ServiceResponse{T}"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="serviceResponse">The service response.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Boolean(ServiceResponse<T> serviceResponse)
        {
            if (serviceResponse == null)
            {
                return false;
            }

            if (serviceResponse.Errors.Any())
            {
                return false;
            }

            if (typeof(T) == typeof(Boolean))
            {
                return Convert.ToBoolean(serviceResponse.Data);
            }

            return serviceResponse.Data != null;
        }

        #region Implementation of IDisposable

        protected Boolean IsDisposed { get; set; }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceResponse{T}" /> class.
        /// </summary>
        ~ServiceResponse()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposeManagedResources)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
            }

            IsDisposed = true;
        }

        #endregion
    }

    public class Error
    {
        
    }

    public class User
    {
        public Int32 Id { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

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
        public String AddressLine1 { get; set; }

        public String AddressLine2 { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String ZipCode { get; set; }

        public String Country { get; set; }
    }
}