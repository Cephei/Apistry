namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Web.Http.Controllers;

    public class DtoPropertyDocumentationMetadataBuilder<TDto, TProperty>
    {
        private readonly DtoDocumentationMetadataBuilder<TDto> _DtoDocumentationMetadataBuilder;

        private readonly PropertyDescriptor _Property;

        private readonly Optional<String> _Description;

        private readonly Optional<TProperty> _ExampleValue;

        private readonly Optional<ISet<HttpMethod>> _RequiredMethods;

        public DtoPropertyDocumentationMetadataBuilder(DtoDocumentationMetadataBuilder<TDto> dtoDocumentationMetadataBuilder, PropertyDescriptor property)
        {
            _DtoDocumentationMetadataBuilder = dtoDocumentationMetadataBuilder;
            _Property = property;

            _Description = new Optional<string>(String.Empty);
            _ExampleValue = new Optional<TProperty>(default(TProperty));
            _RequiredMethods = new Optional<ISet<HttpMethod>>(null);
        }

        public static implicit operator WebApiDocumentationMetadata(DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> builder)
        {
            builder._DtoDocumentationMetadataBuilder.AddDocumentedProperty(builder);
            builder._DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(builder._DtoDocumentationMetadataBuilder);

            return builder._DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder;
        }

        public static implicit operator DtoPropertyDocumentationMetadata(DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> builder)
        {
            return new DtoPropertyDocumentationMetadata(
                builder._Property,
                builder._Description.Value,
                builder._ExampleValue.Value,
                builder._RequiredMethods.Value);
        }

        public HttpControllerDocumentationMetadataBuilder<TApiController> DocumentController<TApiController>() where TApiController : IHttpController
        {
            _DtoDocumentationMetadataBuilder.AddDocumentedProperty(this);

            _DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(_DtoDocumentationMetadataBuilder);

            return _DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.DocumentController<TApiController>();
        }

        public DtoDocumentationMetadataBuilder<TOtherDto> DocumentDto<TOtherDto>(String summary)
        {
            _DtoDocumentationMetadataBuilder.AddDocumentedProperty(this);

            _DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(_DtoDocumentationMetadataBuilder);

            return _DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.DocumentDto<TOtherDto>(summary);
        }

        public DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> Description(String description)
        {
            if (description == null) return this;

            _Description.Value = StringHelper.RemoveMultipleSpaces(description);

            return this;
        }

        public DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> Example(TProperty exampleValue)
        {
            _ExampleValue.Value = exampleValue;

            return this;
        }

        public DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> IsRequired(params HttpMethod[] httpMethods)
        {
            if (httpMethods == null)
            {
                _RequiredMethods.Value = null;
                return this;
            }

            if (!httpMethods.Any())
            {
                _RequiredMethods.Value = new HashSet<HttpMethod>();
                return this;
            }

            _RequiredMethods.Value = new HashSet<HttpMethod>(httpMethods);
            return this;
        }

        public DtoPropertyDocumentationMetadataBuilder<TDto, TOtherProperty> For<TOtherProperty>(Expression<Func<TDto, TOtherProperty>> propertyExpression)
        {
            _DtoDocumentationMetadataBuilder.AddDocumentedProperty(this);

            if (propertyExpression == null)
            {
                throw new InvalidOperationException();
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            var property = TypeDescriptor.GetProperties(typeof(TDto)).Find(memberExpression.Member.Name, true);
            if (_DtoDocumentationMetadataBuilder.Contains(property))
            {
                throw new InvalidOperationException("You cannot configure the same property more than once.");
            }

            return new DtoPropertyDocumentationMetadataBuilder<TDto, TOtherProperty>(_DtoDocumentationMetadataBuilder, property);
        }

        public void Build()
        {
            if (!_DtoDocumentationMetadataBuilder.Contains(_Property))
            {
                _DtoDocumentationMetadataBuilder.AddDocumentedProperty(this);
                _DtoDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(_DtoDocumentationMetadataBuilder);
            }
        }
    }
}