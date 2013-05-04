

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
        private static Lazy<JsonSerializer> _JsonSerializer =
            new Lazy<JsonSerializer>(
                () => JsonSerializer.Create(
                    new JsonSerializerSettings
                        {
                            Formatting = Formatting.None
                        }));

        
        public ActionResult Index()
        {
            var docProvider = (WebApiDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();
            
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

            var apiActionDescriptions = apiExplorer.ApiDescriptions
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(CreateApiActionDescription)
                .ToList();
            
            return View(apiActionDescriptions);
        }

        private static ApiActionDescription CreateApiActionDescription(ApiDescription description)
        {
            return new ApiActionDescription(
                description,
                _JsonSerializer.Value
                               .Deserialize<HttpActionDocumentation>(
                                   new JsonTextReader(new StringReader(description.Documentation))));
        }
    }
}