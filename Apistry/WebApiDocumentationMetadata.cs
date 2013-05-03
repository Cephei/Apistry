namespace Apistry
{
    using System;
    using System.Collections.Generic;

    public class WebApiDocumentationMetadata
    {
        private readonly IDictionary<Type, DtoDocumentationMetadata> _DtoDocumentation;

        private readonly IDictionary<Type, HttpControllerDocumentationMetadata> _ApiControllerDocumentation;

        public WebApiDocumentationMetadata(IDictionary<Type, DtoDocumentationMetadata> dtoDocumentation, IDictionary<Type, HttpControllerDocumentationMetadata> apiControllerDocumentation)
        {
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
    }
}