namespace Apistry.Samples.Service.Api.Patching
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Apistry.Samples.Application.Dto;
    using NContext.Common;

    /// <summary>
    /// Defines a transfer object for HTTP PATCH support.
    /// </summary>
    public class PatchRequest<TDto> : Dictionary<String, Object>
    {
        public PatchRequest() : base(new CaseInsensitiveEqualityComparer())
        {
            
        }

        private class CaseInsensitiveEqualityComparer : IEqualityComparer<String>
        {
            public Boolean Equals(String x, String y)
            {
                return x.ToLowerInvariant().Equals(y.ToLowerInvariant());
            }

            public Int32 GetHashCode(String obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
        }

        public IResponseTransferObject<PatchResult<TDto>> Patch(TDto objectToPatch)
        {
            // TODO: Refactor this entire method! Hack!

            var properties = TypeDescriptor.GetProperties(objectToPatch).Cast<PropertyDescriptor>();
            var patchableProperties = properties.Where(prop => prop.Attributes.OfType<WritableAttribute>().Any());
            var results = new List<PatchOperation<TDto>>();
            var errors = new List<Error>();

            foreach (var property in patchableProperties)
            {
                if (!ContainsKey(property.Name))
                {
                    continue;
                }

                var newPropertyValue = this[property.Name];
                var oldPropertyValue = property.GetValue(objectToPatch);

                if (property.PropertyType.IsEnum)
                {
                    newPropertyValue = Enum.Parse(property.PropertyType, newPropertyValue.ToString());
                }

                try
                {
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        if (newPropertyValue == null || (String.IsNullOrWhiteSpace(newPropertyValue.ToString())))
                        {
                            property.SetValue(objectToPatch, default(DateTime?));
                        }
                        else
                        {
                            property.SetValue(objectToPatch, Convert.ToDateTime(newPropertyValue));
                        }
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(objectToPatch, (Int32)Convert.ChangeType(newPropertyValue, typeof(Int32)));
                    }

                    else if (property.PropertyType == typeof(int?))
                    {
                        int number;
                        Boolean result = Int32.TryParse(newPropertyValue.ToString(), out number);

                        if (result)
                        {
                            property.SetValue(objectToPatch, (Int32?)number);
                        }

                        else
                        {
                            property.SetValue(objectToPatch, null);
                        }
                    }

                    else
                    {
                        property.SetValue(objectToPatch, newPropertyValue);
                    }

                    results.Add(new PatchOperation<TDto>(property.Name, oldPropertyValue, newPropertyValue));
                }
                catch (Exception ex)
                {
                    errors.Add(new ValidationError(typeof(TDto), new List<string> { "Input for the field '" + property.DisplayName + "' is invalid." }));
                }
            }

            if (errors.Any())
            {
                return new ServiceResponse<PatchResult<TDto>>(errors);
            }

            return new ServiceResponse<PatchResult<TDto>>(new PatchResult<TDto>(objectToPatch, results));
        }
    }
}