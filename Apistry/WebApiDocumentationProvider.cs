namespace Apistry
{
    using System;
    using System.Collections;
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
    using FakeItEasy;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoFakeItEasy;
    using Ploeh.AutoFixture.Kernel;

    public class WebApiDocumentationProvider : IDocumentationProvider
    {
        private readonly WebApiDocumentationMetadata _WebApiDocumentationMetadata;
        private readonly JsonSerializer _JsonSerializer;
        private readonly Lazy<IFixture> _Fixture = new Lazy<IFixture>(() =>
            {
                var fixture = new Fixture().Customize(new MultipleCustomization());
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
                        Formatting = Formatting.Indented
                    });}

        public String GetDocumentation(HttpActionDescriptor httpActionDescriptor)
        {
            if (!_WebApiDocumentationMetadata.ApiControllerDocumentation.ContainsKey(httpActionDescriptor.ControllerDescriptor.ControllerType))
            {
                return null;
            }

            var reflectedActionDescriptor = (ReflectedHttpActionDescriptor)httpActionDescriptor;
            var actionDocumentationMetadata = _WebApiDocumentationMetadata
                .ApiControllerDocumentation
                .SingleOrDefault(controller => controller.Key.Equals(httpActionDescriptor.ControllerDescriptor.ControllerType))
                .Value
                .HttpActionDocumentationMetadata
                .SingleOrDefault(action => action.Method.Equals(reflectedActionDescriptor.MethodInfo));

            if (actionDocumentationMetadata == null)
            {
                return null;
            }

            var actionRequestDocumentation = CreateHttpActionRequestDocumentation(reflectedActionDescriptor, actionDocumentationMetadata);
            var actionResponseDocumentation = CreateHttpActionResponseDocumentation(actionDocumentationMetadata);
            var actionDocumentation =
                new HttpActionDocumentation(actionDocumentationMetadata.Name,
                                            actionDocumentationMetadata.Summary,
                                            actionDocumentationMetadata.Alert,
                                            actionDocumentationMetadata.Information,
                                            actionRequestDocumentation,
                                            actionResponseDocumentation);

            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                _JsonSerializer.Serialize(jsonTextWriter, actionDocumentation);

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public String GetDocumentation(HttpParameterDescriptor httpParameterDescriptor)
        {
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

            IDictionary<String, Object> requestBodyExample = null;
            if (formatterParameterBinding != null && _WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(formatterParameterBinding.Descriptor.ParameterType))
            {
                var dtoDocumentationMetadata = _WebApiDocumentationMetadata.DtoDocumentation[formatterParameterBinding.Descriptor.ParameterType];
                requestBodyExample = CreateRequestBodyExample(httpActionDescriptor, dtoDocumentationMetadata);
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
                else if (dtoProperty.ExampleValue != null && 
                         httpActionDescriptor.SupportedHttpMethods
                                                      .Intersect(dtoProperty.RequiredForHttpMethods ?? Enumerable.Empty<HttpMethod>()).Any())
                {
                    requestBodyExample[dtoProperty.Property.Name] = dtoProperty.ExampleValue;
                }
            }

            return requestBodyExample;
        }

        private HttpActionResponseDocumentation CreateHttpActionResponseDocumentation(HttpActionDocumentationMetadata actionDocumentationMetadata)
        {
            if (actionDocumentationMetadata.HttpActionResponseDocumentationMetadata == null ||
                actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Type == null ||
                !_WebApiDocumentationMetadata.DtoDocumentation.ContainsKey(actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Type))
            {
                return null;
            }

            DtoDocumentationMetadata dtoDocumentationMetadata =
                _WebApiDocumentationMetadata.DtoDocumentation[actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Type];

            // Set the response status code.
            HttpStatusCode statusCode = actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.StatusCode;

            // Set the summary which describes the data returned by the HTTP action.
            var responseSummary = actionDocumentationMetadata.HttpActionResponseDocumentationMetadata.Summary ??
                                  dtoDocumentationMetadata.Summary;

            // Generate documentation for the response object's properties.
            var propertyDescriptors = TypeDescriptor.GetProperties(dtoDocumentationMetadata.Type);
            var responseProperties = 
                dtoDocumentationMetadata.Type
                                             .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                             .Where(pi => pi.GetCustomAttributes(true)
                                                            .All(attribute =>
                                                                 !(attribute is NonSerializedAttribute) &&
                                                                 !(attribute is IgnoreDataMemberAttribute)))
                                             .Select(pi => propertyDescriptors[pi.Name])
                                             .ToList();

            IList<PropertyDocumentation> documentedProperties = new List<PropertyDocumentation>();
            foreach (var responsePropertyDescriptor in responseProperties)
            {
                var propertyDocumentationMetadata = dtoDocumentationMetadata
                    .PropertyDocumentationMetadata
                    .SingleOrDefault(pd => pd.Property.Equals(responsePropertyDescriptor));

                var propertyDescription = propertyDocumentationMetadata == null ? String.Empty : propertyDocumentationMetadata.Description;

                documentedProperties.Add(
                    new PropertyDocumentation(
                        responsePropertyDescriptor.Name,
                        responsePropertyDescriptor.PropertyType.Name,
                        propertyDescription));
            }

            // Create an example response object.
            var responseExample = CreateResponseContentExample(dtoDocumentationMetadata);

            return new HttpActionResponseDocumentation(
                responseSummary,
                statusCode,
                documentedProperties,
                responseExample);
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

            return _Fixture.Value.Create(propertyDescriptor.PropertyType, new SpecimenContext(_Fixture.Value));
        }
    }
}