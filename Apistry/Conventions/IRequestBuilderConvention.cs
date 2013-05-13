namespace Apistry.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection;

    public interface IRequestBuilderConvention
    {
        Boolean IncludeProperty(IEnumerable<HttpMethod> httpMethods, PropertyInfo propertyInfo, Type parentObjectType);
    }
}