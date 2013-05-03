namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class HttpActionResponseDocumentation
    {
        private readonly String _Summary;
        private readonly HttpStatusCode _StatusCode;
        private readonly IEnumerable<PropertyDocumentation> _Properties;
        private readonly Object _Content;

        public HttpActionResponseDocumentation(String summary, HttpStatusCode statusCode, IEnumerable<PropertyDocumentation> properties, Object content)
        {
            _Summary = summary;
            _StatusCode = statusCode;
            _Properties = properties;
            _Content = content;
        }

        public String Summary
        {
            get { return _Summary; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
        }

        public IEnumerable<PropertyDocumentation> Properties
        {
            get { return _Properties; }
        }

        public Object Content
        {
            get { return _Content; }
        }
    }
}