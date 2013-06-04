namespace Apistry.Samples.Service.Api.Documentation
{
    using System.ComponentModel.Composition;

    [InheritedExport]
    public interface IBuildWebApiDocumentationMetadata
    {
        void Build(WebApiDocumentationMetadataBuilder metadata);
    }
}