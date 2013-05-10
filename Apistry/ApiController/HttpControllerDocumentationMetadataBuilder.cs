namespace Apistry.ApiController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web.Http.Controllers;

    public class HttpControllerDocumentationMetadataBuilder<TApiController> where TApiController : IHttpController
    {
        private readonly WebApiDocumentationMetadataBuilder _WebApiDocumentationMetadataBuilder;

        private readonly ISet<HttpActionDocumentationMetadata> _HttpActionDocumentation;

        private String _ResourceName;

        private String _Summary;

        public HttpControllerDocumentationMetadataBuilder(WebApiDocumentationMetadataBuilder webApiDocumentationMetadataBuilder)
        {
            _WebApiDocumentationMetadataBuilder = webApiDocumentationMetadataBuilder;
            _HttpActionDocumentation = new HashSet<HttpActionDocumentationMetadata>();
        }

        public static implicit operator WebApiDocumentationMetadata(HttpControllerDocumentationMetadataBuilder<TApiController> metadataBuilder)
        {
            metadataBuilder._WebApiDocumentationMetadataBuilder.AddDocumentedApiController<TApiController>(metadataBuilder);

            return metadataBuilder._WebApiDocumentationMetadataBuilder;
        }

        public static implicit operator HttpControllerDocumentationMetadata(HttpControllerDocumentationMetadataBuilder<TApiController> metadataBuilder)
        {
            return new HttpControllerDocumentationMetadata(typeof(TApiController), metadataBuilder._ResourceName, metadataBuilder._Summary, metadataBuilder._HttpActionDocumentation);
        }

        protected internal WebApiDocumentationMetadataBuilder WebApiDocumentationMetadataBuilder
        {
            get { return _WebApiDocumentationMetadataBuilder; }
        }

        public HttpControllerDocumentationMetadataBuilder<TApiController> Resource(String resourceName)
        {
            _ResourceName = resourceName.Trim();

            return this;
        }

        public HttpControllerDocumentationMetadataBuilder<TApiController> Summary(String summary)
        {
            _Summary = StringHelper.RemoveMultipleSpaces(summary);

            return this;
        }

        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeAction(Expression<Action<TApiController>> httpActionExpression)
        {
            if (httpActionExpression == null)
            {
                throw new InvalidOperationException();
            }

            var methodCallExpression = httpActionExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new InvalidOperationException();
            }

            var method = typeof(TApiController)
                .GetMethod(
                    methodCallExpression.Method.Name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase |
                    BindingFlags.DeclaredOnly,
                    null,
                    CallingConventions.Any,
                    methodCallExpression.Arguments.Select(arg => arg.Type).ToArray(),
                    null);

            if (Contains(method))
            {
                throw new InvalidOperationException(String.Format("You cannot configure the same action, '{0}', more than once.", methodCallExpression.Method.Name));
            }

            return new HttpActionDocumentationMetadataBuilder<TApiController>(this, method);
        }
    
        public HttpActionDocumentationMetadataBuilder<TApiController> DescribeAction<TAction>(Expression<Func<TApiController, TAction>> httpActionExpression)
        {
            if (httpActionExpression == null)
            {
                throw new InvalidOperationException();
            }

            var methodCallExpression = httpActionExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new InvalidOperationException();
            }

            var method = typeof(TApiController)
                .GetMethod(
                    methodCallExpression.Method.Name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase |
                    BindingFlags.DeclaredOnly,
                    null,
                    CallingConventions.Any,
                    methodCallExpression.Arguments.Select(arg => arg.Type).ToArray(),
                    null);

            if (Contains(method))
            {
                throw new InvalidOperationException(String.Format("You cannot configure the same action, '{0}', more than once.", methodCallExpression.Method.Name));
            }

            return new HttpActionDocumentationMetadataBuilder<TApiController>(this, method);
        }

        protected internal Boolean Contains(MethodInfo method)
        {
            return _HttpActionDocumentation.Any(actionDocumentation => actionDocumentation.Method.Equals(method));
        }

        protected internal void AddDocumentedHttpAction(HttpActionDocumentationMetadata httpActionDocumentationMetadata)
        {
            _HttpActionDocumentation.Add(httpActionDocumentationMetadata);
        }

        public void Build()
        {
            if (!WebApiDocumentationMetadataBuilder.Contains(this))
            {
                WebApiDocumentationMetadataBuilder.AddDocumentedApiController<TApiController>(this);
            }
        }
    }
}