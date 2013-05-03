namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class HttpActionDocumentationMetadata
    {
        private readonly MethodInfo _Method;

        private readonly String _Name;

        private readonly String _Summary;

        private readonly HttpActionResponseDocumentationMetadata _HttpActionResponseDocumentationMetadata;

        private readonly IEnumerable<HttpActionParameterDocumentationMetadata> _HttpActionParameters;

        private readonly String _Alert;

        private readonly String _Information;

        public HttpActionDocumentationMetadata(
            MethodInfo method,
            String name,
            String summary,
            HttpActionResponseDocumentationMetadata httpActionResponseDocumentationMetadata,
            IEnumerable<HttpActionParameterDocumentationMetadata> httpActionParameterSet,
            String alert,
            String information)
        {
            _Method = method;
            _Name = name;
            _Summary = summary;
            _HttpActionResponseDocumentationMetadata = httpActionResponseDocumentationMetadata;
            _HttpActionParameters = new HashSet<HttpActionParameterDocumentationMetadata>(httpActionParameterSet);
            _Alert = alert;
            _Information = information;
        }

        public MethodInfo Method
        {
            get { return _Method; }
        }

        public String Name
        {
            get { return _Name; }
        }

        public String Summary
        {
            get { return _Summary; }
        }

        public HttpActionResponseDocumentationMetadata HttpActionResponseDocumentationMetadata
        {
            get { return _HttpActionResponseDocumentationMetadata; }
        }

        public IEnumerable<HttpActionParameterDocumentationMetadata> HttpActionParameters
        {
            get { return _HttpActionParameters; }
        }

        public String Alert
        {
            get { return _Alert; }
        }

        public String Information
        {
            get { return _Information; }
        }
    }
}