namespace Apistry.Dto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net.Http;

    public class DtoPropertyDocumentationMetadata
    {
        private readonly PropertyDescriptor _Property;

        private readonly String _Description;

        private readonly Object _ExampleValue;

        private readonly IEnumerable<HttpMethod> _ExcludedMethods;

        public DtoPropertyDocumentationMetadata(PropertyDescriptor property, String description, Object exampleValue, IEnumerable<HttpMethod> excludedMethods)
        {
            _Property = property;
            _Description = description;
            _ExampleValue = exampleValue;
            _ExcludedMethods = excludedMethods;
        }

        public PropertyDescriptor Property
        {
            get { return _Property; }
        }

        public String Description
        {
            get { return _Description; }
        }

        public Object ExampleValue
        {
            get { return _ExampleValue; }
        }

        public IEnumerable<HttpMethod> ExcludedMethods
        {
            get { return _ExcludedMethods; }
        }
    }
}