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
    public class PatchRequest
    {
        /// <summary>
        /// Gets or sets the properties to patch with the specified values.
        /// </summary>
        public Dictionary<String, Object> SetParameters { get; set; }
    }
}