namespace Apistry
{
    using System;

    public class HttpActionRequestParameterDocumentation
    {
        private readonly String _Name;
        private readonly String _Type;
        private readonly String _Description;
        private readonly Boolean _IsRequired;

        public HttpActionRequestParameterDocumentation(String name, String type, String description, Boolean isRequired)
        {
            _Name = name;
            _Type = type;
            _Description = description;
            _IsRequired = isRequired;
        }

        public String Name
        {
            get { return _Name; }
        }

        public String Type
        {
            get { return _Type; }
        }

        public String Description
        {
            get { return _Description; }
        }

        public Boolean IsRequired
        {
            get { return _IsRequired; }
        }
    }
}