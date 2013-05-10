namespace Apistry.ApiController
{
    using System;
    using System.Collections.Generic;

    public class HttpControllerDocumentationMetadata
    {
        private readonly Type _ApiControllerType;

        private readonly String _ResourceName;

        private readonly String _Summary;

        private readonly ISet<HttpActionDocumentationMetadata> _HttpActionDocumentationMetadata;

        public HttpControllerDocumentationMetadata(Type apiControllerType, String resourceName, String summary, ISet<HttpActionDocumentationMetadata> httpActionDocumentationMetadata)
        {
            _ApiControllerType = apiControllerType;
            _ResourceName = resourceName;
            _Summary = summary;
            _HttpActionDocumentationMetadata = httpActionDocumentationMetadata;
        }

        public Type ApiControllerType
        {
            get { return _ApiControllerType; }
        }

        public ISet<HttpActionDocumentationMetadata> HttpActionDocumentationMetadata
        {
            get { return _HttpActionDocumentationMetadata; }
        }

        public String ResourceName
        {
            get { return _ResourceName; }
        }

        public String Summary
        {
            get { return _Summary; }
        }
    }
}