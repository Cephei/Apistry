namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using Apistry.ApiController;
    using Apistry.Dto;

    public class WebApiDocumentationMetadata
    {
        private readonly ApistrySettings _ApistrySettings;
        private readonly IDictionary<Type, DtoDocumentationMetadata> _DtoDocumentation;
        private readonly IDictionary<Type, HttpControllerDocumentationMetadata> _ApiControllerDocumentation;

        public WebApiDocumentationMetadata(ApistrySettings apistrySettings, IDictionary<Type, DtoDocumentationMetadata> dtoDocumentation, IDictionary<Type, HttpControllerDocumentationMetadata> apiControllerDocumentation)
        {
            _ApistrySettings = apistrySettings ?? new ApistrySettings();
            _DtoDocumentation = dtoDocumentation;
            _ApiControllerDocumentation = apiControllerDocumentation;
        }

        public IDictionary<Type, DtoDocumentationMetadata> DtoDocumentation
        {
            get { return _DtoDocumentation; }
        }

        public IDictionary<Type, HttpControllerDocumentationMetadata> ApiControllerDocumentation
        {
            get { return _ApiControllerDocumentation; }
        }

        protected internal ApistrySettings Settings
        {
            get { return _ApistrySettings; }
        }
    }
}