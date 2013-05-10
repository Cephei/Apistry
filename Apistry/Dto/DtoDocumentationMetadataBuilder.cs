namespace Apistry.Dto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    public class DtoDocumentationMetadataBuilder<TDto>
    {
        private readonly WebApiDocumentationMetadataBuilder _WebApiDocumentationMetadataBuilder;

        private readonly Optional<String> _Summary;

        private readonly ISet<DtoPropertyDocumentationMetadata> _PropertyDocumentation;

        public DtoDocumentationMetadataBuilder(WebApiDocumentationMetadataBuilder webApiDocumentationMetadataBuilder, String summary)
        {
            _WebApiDocumentationMetadataBuilder = webApiDocumentationMetadataBuilder;
            _Summary = new Optional<String>(summary ?? String.Empty);
            _PropertyDocumentation = new HashSet<DtoPropertyDocumentationMetadata>();
        }

        public static implicit operator WebApiDocumentationMetadata(DtoDocumentationMetadataBuilder<TDto> builder)
        {
            builder._WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(builder);

            return builder._WebApiDocumentationMetadataBuilder;
        }

        public static implicit operator DtoDocumentationMetadata(DtoDocumentationMetadataBuilder<TDto> builder)
        {
            return new DtoDocumentationMetadata(typeof(TDto), builder._Summary.Value, builder._PropertyDocumentation);
        }

        protected internal WebApiDocumentationMetadataBuilder WebApiDocumentationMetadataBuilder
        {
            get { return _WebApiDocumentationMetadataBuilder; }
        }

        public DtoPropertyDocumentationMetadataBuilder<TDto, TProperty> For<TProperty>(Expression<Func<TDto, TProperty>> propertyExpression)
        {
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
            if (Contains(property))
            {
                throw new InvalidOperationException("You cannot configure the same property more than once.");
            }

            return new DtoPropertyDocumentationMetadataBuilder<TDto, TProperty>(this, property);
        }

        protected internal Boolean Contains(PropertyDescriptor property)
        {
            return _PropertyDocumentation.Any(pd => pd.Property.Equals(property));
        }

        protected internal void AddDocumentedProperty(DtoPropertyDocumentationMetadata dtoPropertyDocumentationMetadata)
        {
            _PropertyDocumentation.Add(dtoPropertyDocumentationMetadata);
        }

        public void Build()
        {
            if (!WebApiDocumentationMetadataBuilder.Contains(this))
            {
                WebApiDocumentationMetadataBuilder.AddDocumentedDto<TDto>(this);
            }
        }
    }
}