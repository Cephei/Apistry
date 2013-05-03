namespace Apistry
{
    using System;

    public class PropertyDocumentation
    {
        private readonly String _Name;
        private readonly String _Type;
        private readonly String _Description;

        public PropertyDocumentation(String name, String type, String description)
        {
            _Name = name;
            _Type = type;
            _Description = description;
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
    }
}