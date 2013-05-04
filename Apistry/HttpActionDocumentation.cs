namespace Apistry
{
    using System;

    public class HttpActionDocumentation
    {
        private readonly HttpControllerDocumentation _Controller;
        private readonly String _Name;
        private readonly String _Summary;
        private readonly String _Alert;
        private readonly String _Information;
        private readonly HttpActionRequestDocumentation _Request;
        private readonly HttpActionResponseDocumentation _Response;

        public HttpActionDocumentation(HttpControllerDocumentation controller, String name, String summary, String alert, String information, HttpActionRequestDocumentation request, HttpActionResponseDocumentation response)
        {
            _Controller = controller;
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

        public HttpControllerDocumentation Controller
        {
            get { return _Controller; }
        }
    }
}