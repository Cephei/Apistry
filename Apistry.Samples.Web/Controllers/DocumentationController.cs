

namespace Apistry.Samples.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.IO;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Newtonsoft.Json;

    public class DocumentationController : Controller
    {
        private static readonly Lazy<JsonSerializer> _JsonSerializer =
            new Lazy<JsonSerializer>(
                () => JsonSerializer.Create(
                    new JsonSerializerSettings
                        {
                            Formatting = Formatting.None
                        }));

        public static JsonSerializer JsonSerializer
        {
            get { return _JsonSerializer.Value; }
        }


        public ActionResult Index()
        {
            var docProvider = (WebApiDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

            var apiActionDescriptions = apiExplorer.ApiDescriptions
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(description => CreateApiActionDescription(description, docProvider))
                .Where(description => description.Documentation != null)
                .ToList();
            
            return View(apiActionDescriptions);
        }

        private static ApiActionDescription CreateApiActionDescription(ApiDescription description, WebApiDocumentationProvider provider)
        {
            return new ApiActionDescription(
                description,
                provider.GetHttpActionDocumentation(description.ActionDescriptor));
        }
    }
}