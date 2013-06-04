

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
        public ActionResult Index()
        {
            var docProvider = (WebApiDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

            var apiActionDescriptions = apiExplorer.ApiDescriptions
                .AsParallel()
                .WithDegreeOfParallelism(1) // for debugging
                //.WithDegreeOfParallelism(Environment.ProcessorCount) // production
                .Select(description => CreateApiActionDescription(description, docProvider))
                .ToList();

            // You should probably cache the above documentation and generate it only when needed.

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