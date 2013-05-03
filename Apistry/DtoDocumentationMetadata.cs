namespace Apistry
{
    using System;
    using System.Collections.Generic;

    public class DtoDocumentationMetadata
    {
        private readonly Type _Type;

        private readonly String _Summary;

        private readonly IEnumerable<DtoPropertyDocumentationMetadata> _PropertyDocumentationMetadata;

        public DtoDocumentationMetadata(Type type, String summary, IEnumerable<DtoPropertyDocumentationMetadata> propertyDocumentationMetadata)
        {
            _Type = type;
            _Summary = summary;
            _PropertyDocumentationMetadata = propertyDocumentationMetadata;
        }

        public Type Type
        {
            get { return _Type; }
        }

        public IEnumerable<DtoPropertyDocumentationMetadata> PropertyDocumentationMetadata
        {
            get { return _PropertyDocumentationMetadata; }
        }

        public String Summary
        {
            get { return _Summary; }
        }
    }
}