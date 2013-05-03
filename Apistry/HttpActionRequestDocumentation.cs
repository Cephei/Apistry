namespace Apistry
{
    using System;
    using System.Collections.Generic;

    public class HttpActionRequestDocumentation
    {
        private readonly IEnumerable<HttpActionRequestParameterDocumentation> _Parameters;
        private readonly Object _Content;

        public HttpActionRequestDocumentation(IEnumerable<HttpActionRequestParameterDocumentation> parameters, Object content)
        {
            _Parameters = parameters;
            _Content = content;
        }

        public IEnumerable<HttpActionRequestParameterDocumentation> Parameters
        {
            get { return _Parameters; }
        }

        public Object Content
        {
            get { return _Content; }
        }
    }
}