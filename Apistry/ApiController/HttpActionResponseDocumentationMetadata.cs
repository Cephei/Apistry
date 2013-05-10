namespace Apistry.ApiController
{
    using System;
    using System.Net;

    public class HttpActionResponseDocumentationMetadata
    {
        private readonly Type _Type;

        private readonly HttpStatusCode _StatusCode;

        private readonly String _Summary;

        public HttpActionResponseDocumentationMetadata(HttpStatusCode statusCode, String summary)
        {
            _StatusCode = statusCode;
            _Summary = summary;
        }

        public HttpActionResponseDocumentationMetadata(Type type, HttpStatusCode statusCode, String summary)
        {
            _Type = type;
            _StatusCode = statusCode;
            _Summary = summary;
        }

        public Type Type
        {
            get { return _Type; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
        }

        public String Summary
        {
            get { return _Summary; }
        }
    }
}