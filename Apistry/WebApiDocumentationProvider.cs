namespace Apistry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using Apistry.ApiController;
    using Apistry.Dto;
    using Newtonsoft.Json;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public class WebApiDocumentationProvider : IDocumentationProvider
    {
        private readonly WebApiDocumentationMetadata _WebApiDocumentationMetadata;
        private readonly JsonSerializer _JsonSerializer;
        private readonly Lazy<IFixture> _Fixture = new Lazy<IFixture>(() =>
            {
                var fixture = new Fixture().Customize(new CompositeCustomization(new ObjectHydratorCustomization()));
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                return fixture;
            });

        public WebApiDocumentationProvider(WebApiDocumentationMetadata webApiDocumentationMetadata)
        {
            _WebApiDocumentationMetadata = webApiDocumentationMetadata;
            _JsonSerializer = JsonSerializer.Create(
                new JsonSerializerSettings
                    {
                        Formatting = Formatting.None
                    });
        }
        
        public HttpActionDocumentation GetHttpActionDocumentation(HttpActionDescriptor httpActionDescriptor)
        {
            var reflectedActionDescriptor = (ReflectedHttpActionDescriptor)httpActionDescriptor;
            if (!_WebApiDocumentationMetadata.ApiControllerDocumentation.ContainsKey(httpActionDescriptor.ControllerDescriptor.ControllerType))
            {
                return new HttpActionDocumentation(
                    CreateDefaultControllerDocumentation(reflectedActionDescriptor),
                    reflectedActionDescriptor.ActionName,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    CreateDefaultRequestDocumentation(reflectedActionDescriptor),
                    CreateDefaultResponseDocumentation(reflectedActionDescriptor));
            }

            var controllerDocumentationMetadata = _WebApiDocumentationMetadata
                .ApiControllerDocumentation
                .SingleOrDefault(controller => controller.Key.Equals(httpActionDescriptor.ControllerDescriptor.ControllerType))
                .Value;

            var actionDocumentationMetadata = controllerDocumentationMetadata
                .HttpActionDocumentationMetadata
                .SingleOrDefault(action => action.Method.Equals(reflectedActionDescriptor.MethodInfo));

            if (actionDocumentationMetadata == null)
            {
                return null;
            }

            var controllerDocumentation = new HttpControllerDocumentation(
                controllerDocumentationMetadata.ApiControllerType,
                controllerDocumentationMetadata.ResourceName,
                controllerDocumentationMetadata.Summary);

            var actionRequestDocumentation = CreateHttpActionRequestDocumentation(reflectedActionDescriptor, actionDocumentationMetadata);
            var actionResponseDocumentation = CreateHttpActionResponseDocumentation(actionDocumentationMetadata);

            return 
                new HttpActionDocumentation(controllerDocumentation,
                                            actionDocumentationMetadata.Name,
                                            actionDocumentationMetadata.Summary,
                                            actionDocumentationMetadata.Alert,
                                            actionDocumentationMetadata.Information,
                                            actionRequestDocumentation,
                                            actionResponseDocumentation);
        }

        public String GetDocumentation(HttpActionDescriptor httpActionDescriptor)
        {
            var actionDocumentation = GetHttpActionDocumentation(httpActionDescriptor);
            
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                _JsonSerializer.Serialize(jsonTextWriter, actionDocumentation);

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public String GetDocumentation(HttpParameterDescriptor httpParameterDescriptor)
        {
            if (!_WebApiDocumentationMetadata.ApiControllerDocumentation.ContainsKey(httpParameterDescriptor.ActionDescriptor.ControllerDescriptor.ControllerType))
            {
                return null;
            }

            var reflectedActionDescriptor = (ReflectedHttpActionDescriptor)httpParameterDescriptor.ActionDescriptor;
            var actionDocumentationMetadata = _WebApiDocumentationMetadata
                .ApiControllerDocumentation
                .SingleOrDefault(controller => controller.Key.Equals(httpParameterDescriptor.ActionDescriptor.ControllerDescriptor.ControllerType))
                .Value
                .HttpActionDocumentationMetadata
                .SingleOrDefault(action => action.Method.Equals(reflectedActionDescriptor.MethodInfo));

            if (actionDocumentationMetadata == null)
            {
                return null;
            }

            var parameter = actionDocumentationMetadata
                .HttpActionParameters
                .SingleOrDefault(p => p.Name.Equals(httpParameterDescriptor.ParameterName, StringComparison.OrdinalIgnoreCase));

            if (parameter == null)
            {
                return null;
            }

            if (parameter.Type.IsPrimitive || !String.IsNullOrWhiteSpace(parameter.Description)) // DtoDocumentation override
            {
                return parameter.Description;
            }

            if (!_WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(httpParameterDescriptor.ParameterType))
            {
                return null;
            }

            return _WebApiDocumentationMetadata.DtoDocumentation
                                               .Single(dto => dto.Key.Equals(httpParameterDescriptor.ParameterType))
                                               .Value
                                               .Summary;
        }

        private HttpActionRequestDocumentation CreateHttpActionRequestDocumentation(HttpActionDescriptor httpActionDescriptor, HttpActionDocumentationMetadata actionDocumentationMetadata)
        {
            var requestParameters = CreateHttpActionRequestParameters(httpActionDescriptor, actionDocumentationMetadata);
            var requestContent = CreateHttpActionRequestBody(httpActionDescriptor);

            return new HttpActionRequestDocumentation(requestParameters, requestContent);
        }

        private IEnumerable<HttpActionRequestParameterDocumentation> CreateHttpActionRequestParameters(HttpActionDescriptor httpActionDescriptor, HttpActionDocumentationMetadata actionDocumentationMetadata)
        {
            return
                from parameterDescriptor in httpActionDescriptor.GetParameters()
                where parameterDescriptor.ParameterBinderAttribute is FromUriAttribute || TypeHelper.CanConvertFromString(parameterDescriptor.ParameterType)
                let parameterDescription =
                    actionDocumentationMetadata.HttpActionParameters
                                               .Any(parameterDocumentation =>
                                                    parameterDocumentation.Name.Equals(parameterDescriptor.ParameterName) &&
                                                    parameterDocumentation.Type.Equals(parameterDescriptor.ParameterType))
                        ? actionDocumentationMetadata.HttpActionParameters
                                                     .Single(parameterDocumentation =>
                                                             parameterDocumentation.Name.Equals(parameterDescriptor.ParameterName) &&
                                                             parameterDocumentation.Type.Equals(parameterDescriptor.ParameterType)).Description
                        : String.Empty
                select new HttpActionRequestParameterDocumentation(
                    parameterDescriptor.ParameterName,
                    parameterDescriptor.ParameterType.Name,
                    parameterDescription,
                    !parameterDescriptor.IsOptional);
        }

        private Object CreateHttpActionRequestBody(HttpActionDescriptor httpActionDescriptor)
        {
            var formatterParameterBinding = 
                httpActionDescriptor.ActionBinding
                                    .ParameterBindings
                                    .SingleOrDefault(binding => !binding.Descriptor.IsOptional && 
                                                                !TypeHelper.CanConvertFromString(binding.Descriptor.ParameterType) &&
                                                                binding.WillReadBody);

            if (formatterParameterBinding == null)
            {
                return null;
            }

            IDictionary<String, Object> requestBodyExample;
            var parameterType = GetEnumerableType(formatterParameterBinding.Descriptor.ParameterType);
            if (_WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(parameterType))
            {
                var dtoDocumentationMetadata = _WebApiDocumentationMetadata.DtoDocumentation[parameterType];
                requestBodyExample = CreateRequestBodyExample(httpActionDescriptor, dtoDocumentationMetadata);
            }
            else
            {
                requestBodyExample = CreateDefaultRequestBodyExample(httpActionDescriptor, parameterType);
            }

            return requestBodyExample;
        }

        private IDictionary<String, Object> CreateRequestBodyExample(HttpActionDescriptor httpActionDescriptor, DtoDocumentationMetadata dtoDocumentationMetadata)
        {
            IDictionary<String, Object> requestBodyExample = new ExpandoObject();
            foreach (DtoPropertyDocumentationMetadata dtoProperty in dtoDocumentationMetadata.PropertyDocumentationMetadata)
            {
                if (!TypeHelper.CanConvertFromString(dtoProperty.Property.PropertyType) &&
                    _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(dtoProperty.Property.PropertyType))
                {
                    requestBodyExample[dtoProperty.Property.Name] = 
                        CreateRequestBodyExample(httpActionDescriptor, _WebApiDocumentationMetadata.DtoDocumentation[dtoProperty.Property.PropertyType]);
                }
                else if (!httpActionDescriptor.SupportedHttpMethods.Intersect(dtoProperty.ExcludedMethods ?? Enumerable.Empty<HttpMethod>()).Any())
                {
                    if (_WebApiDocumentationMetadata.Settings
                        .RequestBuilderConventions
                        .Any(c => !c.IncludeProperty(
                            httpActionDescriptor.SupportedHttpMethods, 
                            dtoDocumentationMetadata.Type.GetProperty(dtoProperty.Property.Name), 
                            dtoDocumentationMetadata.Type)))
                    {
                        continue;
                    }

                    requestBodyExample[dtoProperty.Property.Name] = GetOrCreateExampleValue(dtoDocumentationMetadata, dtoProperty.Property);
                }
            }

            return requestBodyExample;
        }

        private HttpActionResponseDocumentation CreateHttpActionResponseDocumentation(HttpActionDocumentationMetadata actionDocumentationMetadata)
        {
            if (actionDocumentationMetadata.HttpActionResponseDocumentationMetadata == null)
            {
                return null;
            }

            // Set the response status code.
            HttpStatusCode statusCode = actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.StatusCode;

            // Set the summary which describes the data returned by the HTTP action.
            String responseSummary = actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Summary;

            IEnumerable<PropertyDocumentation> documentedProperties = null;
            Object responseExample = null;
            Type responseType = GetResponseType(actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Type);
            if (responseType != null && _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(responseType))
            {
                DtoDocumentationMetadata dtoDocumentationMetadata = _WebApiDocumentationMetadata.DtoDocumentation[responseType];
                
                // Set the summary which describes the data returned by the HTTP action.
                responseSummary = responseSummary ?? dtoDocumentationMetadata.Summary;

                // Generate documentation for the response object's properties.
                documentedProperties = CreatePropertyDocumentation(dtoDocumentationMetadata, String.Empty);

                // Create an example response object.
                responseExample = CreateResponseContentExample(dtoDocumentationMetadata);
            }

            return new HttpActionResponseDocumentation(
                responseSummary, 
                statusCode, 
                documentedProperties ?? Enumerable.Empty<PropertyDocumentation>(), 
                responseExample);
        }

        private IEnumerable<PropertyDocumentation> CreatePropertyDocumentation(DtoDocumentationMetadata dtoDocumentationMetadata, String propertyPrefix)
        {
            if (propertyPrefix == null)
            {
                propertyPrefix = String.Empty;
            }

            IList<PropertyDocumentation> propertyDocumentation = new List<PropertyDocumentation>();
            var propertyDescriptors = TypeDescriptor.GetProperties(dtoDocumentationMetadata.Type)
                                                    .Cast<PropertyDescriptor>()
                                                    .Where(pi => pi.PropertyType.GetCustomAttributes(true)
                                                    .All(attribute =>
                                                            !(attribute is NonSerializedAttribute) &&
                                                            !(attribute is IgnoreDataMemberAttribute)))
                                                    .Select(pi => pi)
                                                    .ToList();

            foreach (var propertyDescriptor in propertyDescriptors)
            {
                var documentedProperty = 
                    dtoDocumentationMetadata.PropertyDocumentationMetadata
                                            .SingleOrDefault(
                                                metadata => 
                                                metadata.Property.Name.Equals(propertyDescriptor.Name) && 
                                                metadata.Property.PropertyType.Equals(propertyDescriptor.PropertyType));

                if (!TypeHelper.CanConvertFromString(propertyDescriptor.PropertyType) &&
                    _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(propertyDescriptor.PropertyType))
                {
                    propertyDocumentation.Add(
                        new PropertyDocumentation(
                            GetPropertyNamePrefix(propertyDescriptor, propertyPrefix),
                            GetProperyTypeName(propertyDescriptor),
                            _WebApiDocumentationMetadata.DtoDocumentation[propertyDescriptor.PropertyType].Summary));

                    propertyDocumentation.AddRange(
                        CreatePropertyDocumentation(
                            _WebApiDocumentationMetadata.DtoDocumentation[propertyDescriptor.PropertyType],
                            String.Format("{0}{1}{2}", propertyPrefix, String.IsNullOrWhiteSpace(propertyPrefix) ? String.Empty : ".", propertyDescriptor.Name)));
                }
                else if (!TypeHelper.CanConvertFromString(propertyDescriptor.PropertyType) &&
                         GetEnumerableType(propertyDescriptor.PropertyType) != null &&
                         _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(propertyDescriptor.PropertyType.GetGenericArguments().First()))
                {
                    var genericPropertyType = propertyDescriptor.PropertyType.GetGenericArguments().First();

                    propertyDocumentation.Add(
                        new PropertyDocumentation(
                            GetPropertyNamePrefix(propertyDescriptor, propertyPrefix),
                            GetProperyTypeName(propertyDescriptor),
                            _WebApiDocumentationMetadata.DtoDocumentation[genericPropertyType].Summary));

                    if (propertyPrefix.Contains(propertyDescriptor.Name) || genericPropertyType.Equals(dtoDocumentationMetadata.Type))
                    {
                        continue; // Prevent circular dependencies.
                    }

                    propertyDocumentation.AddRange(
                        CreatePropertyDocumentation(
                            _WebApiDocumentationMetadata.DtoDocumentation[genericPropertyType],
                            String.Format("{0}{1}{2}", propertyPrefix, String.IsNullOrWhiteSpace(propertyPrefix) ? String.Empty : ".", propertyDescriptor.Name)));
                }
                else
                {
                    propertyDocumentation.Add(
                        new PropertyDocumentation(
                            GetPropertyNamePrefix(propertyDescriptor, propertyPrefix),
                            GetProperyTypeName(propertyDescriptor),
                            documentedProperty == null ? String.Empty : documentedProperty.Description));
                }
            }

            return propertyDocumentation;
        }

        private String GetPropertyNamePrefix(PropertyDescriptor propertyDescriptor, String existingPrefix)
        {
            var propertyName = propertyDescriptor.Name;
            if (propertyDescriptor.PropertyType.IsGenericType)
            {
                if (GetEnumerableType(propertyDescriptor.PropertyType) != null)
                {
                    propertyName += "[]";
                }
            }

            return String.Format(
                "{0}{1}{2}",
                existingPrefix,
                String.IsNullOrWhiteSpace(existingPrefix) ? String.Empty : ".",
                propertyName);
        }

        private String GetProperyTypeName(PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType.IsGenericType)
            {
                if (propertyDescriptor.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return String.Format("{0} (list)", propertyDescriptor.PropertyType.GetGenericArguments().Single().Name);
                }
                
                if (propertyDescriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return String.Format("{0} (nullable)", propertyDescriptor.PropertyType.GetGenericArguments().Single().Name);
                }
            }
            else if (typeof(Byte[]).IsAssignableFrom(propertyDescriptor.PropertyType))
            {
                return "binary";
            }
            else if (!TypeHelper.CanConvertFromString(propertyDescriptor.PropertyType))
            {
                return String.Format("{0} (object)", propertyDescriptor.PropertyType.Name);
            }

            return propertyDescriptor.PropertyType.Name;
        }

        private static Type GetResponseType(Type type)
        {
            return type == null || !type.IsGenericType ? type : GetEnumerableType(type);
        }

        private static Type GetEnumerableType(Type type)
        {
            if (type == typeof(String))
            {
                return typeof(String);
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericArguments().Count() == 1)
                {
                    return type.GetGenericArguments().First();
                }

                return (from intType in type.GetInterfaces()
                        where intType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        select intType.GetGenericArguments()[0]).FirstOrDefault();
            }

            return type;
        }

        private Object CreateResponseContentExample(DtoDocumentationMetadata dtoDocumentationMetadata)
        {
            IDictionary<String, Object> responseExample = new ExpandoObject();
            var propertyDescriptors = TypeDescriptor.GetProperties(dtoDocumentationMetadata.Type);
            var dtoProperties =
                dtoDocumentationMetadata.Type
                                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                        .Where(pi => pi.GetCustomAttributes(true)
                                                       .All(
                                                           attribute =>
                                                           !(attribute is NonSerializedAttribute) &&
                                                           !(attribute is IgnoreDataMemberAttribute)))
                                        .Select(pi => propertyDescriptors[pi.Name]);

            foreach (var propertyDescriptor in dtoProperties)
            {
                if (!TypeHelper.CanConvertFromString(propertyDescriptor.PropertyType))
                {
                    if (_WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(propertyDescriptor.PropertyType))
                    {
                        responseExample[propertyDescriptor.Name] =
                            CreateResponseContentExample(_WebApiDocumentationMetadata.DtoDocumentation[propertyDescriptor.PropertyType]);
                    }
                    else
                    {
                        responseExample[propertyDescriptor.Name] = GetOrCreateExampleValue(null, propertyDescriptor);
                    }
                }
                else
                {
                    responseExample[propertyDescriptor.Name] = GetOrCreateExampleValue(dtoDocumentationMetadata, propertyDescriptor);
                }
            }

            return responseExample;
        }

        private Object GetOrCreateExampleValue(DtoDocumentationMetadata dtoDocumentationMetadata, PropertyDescriptor propertyDescriptor)
        {
            if (dtoDocumentationMetadata != null)
            {
                var propertyDocumentationMetadata =
                    dtoDocumentationMetadata.PropertyDocumentationMetadata
                                            .SingleOrDefault(pdm => pdm.Property.Equals(propertyDescriptor) && pdm.ExampleValue != null);

                if (propertyDocumentationMetadata != null)
                {
                    return propertyDocumentationMetadata.ExampleValue;
                }
            }

            return GetOrCreateExampleValue(propertyDescriptor.PropertyType);
        }

        private Object GetOrCreateExampleValue(Type type)
        {
            try
            {
                return _Fixture.Value.Create(type, new SpecimenContext(_Fixture.Value));
            }
            catch
            {
                return null;
            }
        }

        private HttpControllerDocumentation CreateDefaultControllerDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return new HttpControllerDocumentation(
                actionDescriptor.ControllerDescriptor.ControllerType,
                actionDescriptor.ControllerDescriptor.ControllerName,
                String.Empty);
        }

        private HttpActionRequestDocumentation CreateDefaultRequestDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return new HttpActionRequestDocumentation(
                actionDescriptor.GetParameters()
                                .Where(p => p.ParameterBinderAttribute is FromUriAttribute || TypeHelper.CanConvertFromString(p.ParameterType))
                                .Select(p => new HttpActionRequestParameterDocumentation(p.ParameterName, p.ParameterType.Name, String.Empty, !p.IsOptional)),
                CreateHttpActionRequestBody(actionDescriptor));
        }

        private IDictionary<String, Object> CreateDefaultRequestBodyExample(HttpActionDescriptor httpActionDescriptor, Type dto)
        {
            IDictionary<String, Object> requestBodyExample = new ExpandoObject();
            foreach (var dtoProperty in dto.GetProperties())
            {
                if (!TypeHelper.CanConvertFromString(dtoProperty.PropertyType) &&
                    _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(dtoProperty.PropertyType))
                {
                    requestBodyExample[dtoProperty.Name] =
                        CreateRequestBodyExample(httpActionDescriptor, _WebApiDocumentationMetadata.DtoDocumentation[dtoProperty.PropertyType]);
                }
                else
                {
                    if (_WebApiDocumentationMetadata.Settings
                            .RequestBuilderConventions
                            .Any(c => !c.IncludeProperty(httpActionDescriptor.SupportedHttpMethods, dtoProperty, dto)))
                    {
                        continue;
                    }

                    requestBodyExample[dtoProperty.Name] = GetOrCreateExampleValue(dtoProperty.PropertyType);
                }
            }

            return requestBodyExample;
        }

        private HttpActionResponseDocumentation CreateDefaultResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return new HttpActionResponseDocumentation(
                String.Empty,
                HttpStatusCode.OK,
                Enumerable.Empty<PropertyDocumentation>(),
                null);
        }
    }
}