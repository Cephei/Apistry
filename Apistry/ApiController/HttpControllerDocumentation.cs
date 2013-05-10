namespace Apistry.ApiController
{
    using System;

    public class HttpControllerDocumentation
    {
        private readonly Type _Type;
        private readonly String _ResourceName;
        private readonly String _Summary;

        public HttpControllerDocumentation(Type type, String resourceName, String summary)
        {
            _Type = type;
            _ResourceName = resourceName;
            _Summary = summary;
        }

        public Type Type
        {
            get { return _Type; }
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