namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.Http.Controllers;
    using Apistry.ApiController;
    using Apistry.Dto;

    public class WebApiDocumentationMetadataBuilder
    {
        private readonly ApistrySettings _ApistrySettings;
        private readonly IDictionary<Type, DtoDocumentationMetadata> _DtoDocumentation;
        private readonly IDictionary<Type, HttpControllerDocumentationMetadata> _ApiControllerDocumentation;

        public WebApiDocumentationMetadataBuilder() : this(null)
        {
        }

        public WebApiDocumentationMetadataBuilder(ApistrySettings apistrySettings)
        {
            _ApistrySettings = apistrySettings;
            _DtoDocumentation = new Dictionary<Type, DtoDocumentationMetadata>();
            _ApiControllerDocumentation = new Dictionary<Type, HttpControllerDocumentationMetadata>();
        }

        public static implicit operator WebApiDocumentationMetadata(WebApiDocumentationMetadataBuilder metadataBuilder)
        {
            return new WebApiDocumentationMetadata(metadataBuilder._ApistrySettings, metadataBuilder._DtoDocumentation, metadataBuilder._ApiControllerDocumentation);
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

            return new DtoDocumentationMetadataBuilder<TDto>(this, Regex.Replace(summary.Trim(), @"\s+", " "));
        }

        public Boolean Contains(HttpControllerDocumentationMetadata httpControllerDocumentationMetadata)
        {
            return _ApiControllerDocumentation.ContainsKey(httpControllerDocumentationMetadata.ApiControllerType);
        }

        public Boolean Contains(DtoDocumentationMetadata dtoDocumentationMetadata)
        {
            return _DtoDocumentation.ContainsKey(dtoDocumentationMetadata.Type);
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