namespace Apistry
{
    using System;

    public class HttpActionParameterDocumentationMetadata
    {
        private readonly String _Name;

        private readonly Type _Type;

        private readonly String _Description;

        public HttpActionParameterDocumentationMetadata(String name, Type type, String description)
        {
            _Name = name;
            _Type = type;
            _Description = description;
        }

        public String Name
        {
            get { return _Name; }
        }

        public Type Type
        {
            get { return _Type; }
        }

        public String Description
        {
            get { return _Description; }
        }
    }
}