namespace Apistry.ApiController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;
    using System.Web.Http.Controllers;
    using Apistry.Dto;

    public class HttpActionDocumentationMetadataBuilder<TApiController> where TApiController : IHttpController
    {
        private readonly HttpControllerDocumentationMetadataBuilder<TApiController> _HttpControllerDocumentationMetadataBuilder;

        private readonly MethodInfo _Method;

        private readonly Optional<String> _Name;

        private readonly Optional<String> _Summary;

        private readonly Optional<HttpActionResponseDocumentationMetadata> _ResponseDescriptor;

        private readonly Optional<ISet<HttpActionParameterDocumentationMetadata>> _HttpActionParameters;

        private readonly Optional<String> _Alert;

        private readonly Optional<String> _Information;

        public HttpActionDocumentationMetadataBuilder(HttpControllerDocumentationMetadataBuilder<TApiController> httpControllerDocumentationMetadataBuilder, MethodInfo method)
        {
            _HttpControllerDocumentationMetadataBuilder = httpControllerDocumentationMetadataBuilder;
            _Method = method;

            _Name = new Optional<String>(String.Empty);
            _Summary = new Optional<String>(String.Empty);
            _ResponseDescriptor = new Optional<HttpActionResponseDocumentationMetadata>(null);
            _HttpActionParameters = new Optional<ISet<HttpActionParameterDocumentationMetadata>>(new HashSet<HttpActionParameterDocumentationMetadata>());
            _Alert = new Optional<String>(String.Empty);
            _Information = new Optional<String>(String.Empty);
        }

        public static implicit operator WebApiDocumentationMetadata(HttpActionDocumentationMetadataBuilder<TApiController> builder)
        {
            builder._HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(builder);
            builder._HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedApiController<TApiController>(builder._HttpControllerDocumentationMetadataBuilder);

            return builder._HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder;
        }

        public static implicit operator HttpActionDocumentationMetadata(HttpActionDocumentationMetadataBuilder<TApiController> builder)
        {
            return new HttpActionDocumentationMetadata(
                builder._Method,
                builder._Name.Value,
                builder._Summary.Value,
                builder._ResponseDescriptor.Value,
                builder._HttpActionParameters.Value,
                builder._Alert.Value,
                builder._Information.Value);
        }

        public DtoDocumentationMetadataBuilder<TDto> DocumentDto<TDto>(String summary)
        {
            _HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(this);

            _HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedApiController<TApiController>(_HttpControllerDocumentationMetadataBuilder);

            return _HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.DocumentDto<TDto>(summary);
        }

        public HttpControllerDocumentationMetadataBuilder<TOtherApiController> DocumentController<TOtherApiController>() where TOtherApiController : IHttpController
        {
            _HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(this);

            _HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.AddDocumentedApiController<TApiController>(_HttpControllerDocumentationMetadataBuilder);

            return _HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder.DocumentController<TOtherApiController>();
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeAction(Expression<Action<TApiController>> httpActionExpression)
        {
            _HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(this);

            return _HttpControllerDocumentationMetadataBuilder.DescribeAction(httpActionExpression);
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeAction<TAction>(Expression<Func<TApiController, TAction>> httpActionExpression)
        {
            _HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(this);

            return _HttpControllerDocumentationMetadataBuilder.DescribeAction(httpActionExpression);
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Name(String name)
        {
            if (name == null) return this;

            _Name.Value = name.Trim();

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Summary(String summary)
        {
            if (summary == null) return this;

            _Summary.Value = StringHelper.RemoveMultipleSpaces(summary);

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Returns(HttpStatusCode statusCode = HttpStatusCode.OK, String description = null)
        {
            if (_ResponseDescriptor.Value != _ResponseDescriptor.OriginalValue)
            {
                throw new InvalidOperationException(String.Format("You cannot configure the return value for a HTTP action '{0}' multiple times.", _Method.Name));
            }

            _ResponseDescriptor.Value = new HttpActionResponseDocumentationMetadata(statusCode, description);

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Returns<TDto>(HttpStatusCode statusCode = HttpStatusCode.OK, String description = null)
        {
            if (_ResponseDescriptor.Value != _ResponseDescriptor.OriginalValue)
            {
                throw new InvalidOperationException(String.Format("You cannot configure the return value for a HTTP action '{0}' multiple times.", _Method.Name));
            }

            _ResponseDescriptor.Value = new HttpActionResponseDocumentationMetadata(typeof(TDto), statusCode, description);

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeParameter<TParameterType>(String parameterName)
        {
            return DescribeParameter<TParameterType>(parameterName, null);
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeParameter<TParameterType>(String parameterName, String description)
        {
            if (!_Method.GetParameters().Any(parameter => parameter.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase) && parameter.ParameterType.Equals(typeof(TParameterType))))
            {
                throw new ArgumentException(String.Format("Parameter name: '{0}' with type '{1}' does not exist on method '{2}'.", parameterName, typeof(TParameterType).Name, _Method.Name));
            }

            if (_HttpActionParameters.Value.Any(parameter => parameter.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("You cannot have multiple HTTP action parameters with the same name.");
            }

            _HttpActionParameters.Value.Add(new HttpActionParameterDocumentationMetadata(parameterName, typeof(TParameterType), description));

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Alert(String alert)
        {
            if (alert == null) return this;

            _Alert.Value = StringHelper.RemoveMultipleSpaces(alert);

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> Information(String information)
        {
            if (information == null) return this;

            _Information.Value = StringHelper.RemoveMultipleSpaces(information);

            return this;
        }

        public void Build()
        {
            if (!_HttpControllerDocumentationMetadataBuilder.Contains(_Method))
            {
                _HttpControllerDocumentationMetadataBuilder.AddDocumentedHttpAction(this);
                _HttpControllerDocumentationMetadataBuilder.WebApiDocumentationMetadataBuilder
                                                           .AddDocumentedApiController<TApiController>(_HttpControllerDocumentationMetadataBuilder);
            }
        }
    }
}