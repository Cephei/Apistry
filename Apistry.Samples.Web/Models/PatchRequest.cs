// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchRequest.cs" company="Innovative Data Solutions Inc.">
//   Copyright © 2012 Innovative Data Solutions Inc.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Apistry.Samples.Web.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a transfer object for HTTP PATCH support.
    /// </summary>
    public class PatchRequest<TDto> : Dictionary<String, Object>
    {
    }
}