namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class DtoPropertyDocumentationMetadata
    {
        private readonly PropertyDescriptor _Property;

        private readonly String _Description;

        private readonly Object _ExampleValue;

        private readonly IEnumerable<System.Net.Http.HttpMethod> _RequiredForHttpMethods;

        public DtoPropertyDocumentationMetadata(PropertyDescriptor property, String description, Object exampleValue, IEnumerable<System.Net.Http.HttpMethod> requiredForHttpMethods)
        {
            _Property = property;
            _Description = description;
            _ExampleValue = exampleValue;
            _RequiredForHttpMethods = requiredForHttpMethods;
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

        public IEnumerable<System.Net.Http.HttpMethod> RequiredForHttpMethods
        {
            get { return _RequiredForHttpMethods; }
        }
    }
}