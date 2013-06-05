using System.Text;

namespace Apistry.Samples.Service.Api.Documentation
{
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apistry.Samples.Service.Api.Documentation.Conventions;
    using NContext.Extensions.AspNetWebApi.Configuration;

    internal class DocumentationConfiguration : IConfigureWebApi
    {
        public void Configure(HttpConfiguration configuration)
        {
            var apistrySettings = new ApistrySettings();
            apistrySettings.RequestBuilderConventions.Add(new WritableConvention());
            apistrySettings.RequestBuilderConventions.Add(new EntityIdConvention());

            var metadata = new WebApiDocumentationMetadataBuilder(apistrySettings);
            var container = new CompositionContainer(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            foreach (var documentationBuilder in container.GetExportedValues<IBuildWebApiDocumentationMetadata>())
            {
                documentationBuilder.Build(metadata);
            }

            configuration.Services.Replace(typeof(IDocumentationProvider), new WebApiDocumentationProvider(metadata));
        }
    }
}