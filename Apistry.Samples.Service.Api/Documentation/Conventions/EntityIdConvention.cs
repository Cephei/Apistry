namespace Apistry.Samples.Service.Api.Documentation.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using Apistry.Conventions;

    public class EntityIdConvention : IRequestBuilderConvention
    {
        public Boolean IncludeProperty(IEnumerable<HttpMethod> httpMethods, PropertyInfo propertyInfo, Type parentObjectType)
        {
            return httpMethods.All(m => !m.Equals(HttpMethod.Post)) || !propertyInfo.Name.Equals("Id", StringComparison.OrdinalIgnoreCase);
        }
    }
}