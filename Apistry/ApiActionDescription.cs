namespace Apistry
{
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Routing;

    public class ApiActionDescription
    {
        private readonly ApiDescription _ApiDescription;
        private readonly HttpActionDocumentation _HttpActionDocumentation;

        public ApiActionDescription(ApiDescription apiDescription, HttpActionDocumentation httpActionDocumentation)
        {
            _ApiDescription = apiDescription;
            _HttpActionDocumentation = httpActionDocumentation;
        }

        public HttpActionDocumentation Documentation
        {
            get { return _HttpActionDocumentation;}
        }

        public HttpMethod HttpMethod
        {
            get { return _ApiDescription.HttpMethod; }
        }

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        /// <value>
        /// The relative path.
        /// </value>
        public string RelativePath
        {
            get { return _ApiDescription.RelativePath; }
        }

        /// <summary>
        /// Gets or sets the action descriptor that will handle the API.
        /// </summary>
        /// <value>
        /// The action descriptor.
        /// </value>
        public HttpActionDescriptor ActionDescriptor
        {
            get { return _ApiDescription.ActionDescriptor; }
        }

        /// <summary>
        /// Gets or sets the registered route for the API.
        /// </summary>
        /// <value>
        /// The route.
        /// </value>
        public IHttpRoute Route { get; set; }
        
        /// <summary>
        /// Gets the supported response formatters.
        /// </summary>
        public Collection<MediaTypeFormatter> SupportedResponseFormatters
        {
            get { return _ApiDescription.SupportedResponseFormatters; }
        }

        /// <summary>
        /// Gets the supported request body formatters.
        /// </summary>
        public Collection<MediaTypeFormatter> SupportedRequestBodyFormatters
        {
            get { return _ApiDescription.SupportedRequestBodyFormatters; }
        }

        /// <summary>
        /// Gets the parameter descriptions.
        /// </summary>
        public Collection<ApiParameterDescription> ParameterDescriptions
        {
            get { return _ApiDescription.ParameterDescriptions; }
        }

        /// <summary>
        /// Gets the ID. The ID is unique within <see cref="HttpServer"/>.
        /// </summary>
        public string ID
        {
            get { return _ApiDescription.ID; }
        }
    }
}