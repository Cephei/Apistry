namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;

    public class WebApiDocumentationMetadataBuilder
    {
        private readonly IDictionary<Type, DtoDocumentationMetadata> _DtoDocumentation;

        private readonly IDictionary<Type, HttpControllerDocumentationMetadata> _ApiControllerDocumentation;

        public WebApiDocumentationMetadataBuilder()
        {
            _DtoDocumentation = new Dictionary<Type, DtoDocumentationMetadata>();
            _ApiControllerDocumentation = new Dictionary<Type, HttpControllerDocumentationMetadata>();
        }

        public static implicit operator WebApiDocumentationMetadata(WebApiDocumentationMetadataBuilder metadataBuilder)
        {
            return new WebApiDocumentationMetadata(metadataBuilder._DtoDocumentation, metadataBuilder._ApiControllerDocumentation);
        }

        public HttpControllerDocumentationMetadataBuilder<TApiController> DocumentController<TApiController>() where TApiController : IHttpController
        {
            if (_ApiControllerDocumentation.ContainsKey(typeof(TApiController)))
            {
                throw new InvalidOperationException(String.Format("You cannot document the same IHttpController, '{0}', more than once.", typeof(TApiController).Name));
            }

            return new HttpControllerDocumentationMetadataBuilder<TApiController>(this);
        }

        public DtoDocumentationMetadataBuilder<TDto> DocumentDto<TDto>(String summary)
        {
            if (_DtoDocumentation.ContainsKey(typeof(TDto)))
            {
                throw new InvalidOperationException(String.Format("You cannot document the same DTO, '{0}', more than once.", typeof(TDto).Name));
            }

            return new DtoDocumentationMetadataBuilder<TDto>(this, summary);
        }

        protected internal void AddDocumentedDto<TDto>(DtoDocumentationMetadata dtoDocumentationMetadata)
        {
            _DtoDocumentation.Add(typeof(TDto), dtoDocumentationMetadata);
        }

        protected internal void AddDocumentedApiController<TApiController>(HttpControllerDocumentationMetadata httpControllerDocumentationMetadata)
        {
            _ApiControllerDocumentation.Add(typeof(TApiController), httpControllerDocumentationMetadata);
        }
    }
}