namespace Apistry
{
    using System;

    public class HttpActionDocumentation
    {
        private readonly String _Name;
        private readonly String _Summary;
        private readonly String _Alert;
        private readonly String _Information;
        private readonly HttpActionRequestDocumentation _Request;
        private readonly HttpActionResponseDocumentation _Response;

        public HttpActionDocumentation(String name, String summary, String alert, String information, HttpActionRequestDocumentation request, HttpActionResponseDocumentation response)
        {
            _Name = name;
            _Summary = summary;
            _Alert = alert;
            _Information = information;
            _Request = request;
            _Response = response;
        }

        public String Name
        {
            get { return _Name; }
        }

        public String Summary
        {
            get { return _Summary; }
        }

        /*public HttpActionRequestParameterDocumentation ParameterDocumentation
        {
            get { return _ParameterDocumentation; }
        }*/

        public String Alert
        {
            get { return _Alert; }
        }

        public String Information
        {
            get { return _Information; }
        }

        public HttpActionRequestDocumentation Request
        {
            get { return _Request; }
        }

        public HttpActionResponseDocumentation Response
        {
            get { return _Response; }
        }
    }
}