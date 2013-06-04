// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchRequest.cs" company="Innovative Data Solutions Inc.">
//   Copyright © 2012 Innovative Data Solutions Inc.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Apistry.Samples.Service.Api
{
    using System;
    using System.Collections.Generic;

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
    }
}