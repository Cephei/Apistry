namespace Apistry.Samples.Service.Api.Documentation.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using Apistry.Conventions;
    using Apistry.Samples.Application.Dto;

    public class WritableConvention : IRequestBuilderConvention
    {
        public Boolean IncludeProperty(IEnumerable<HttpMethod> httpMethods, PropertyInfo propertyInfo, Type parentObjectType)
        {
            return httpMethods.All(m => !m.Equals(ApiHttpMethod.Patch)) ||
                   propertyInfo.GetCustomAttributes(typeof(WritableAttribute), true).Any();
        }
    }
}