// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRevisionFilesDisplayController.cs" company="Innovative Data Solutions">
//   Copyright © Innovative Data Solutions, Inc. 2012
// </copyright>
// <summary>
//   Defines the DocumentRevisionFilesDisplayController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ids.Ecm.Service.Api.Policy
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    
    public class DocumentRevisionFilesDisplayController : ApiController
    {
        /// <summary>
        /// Returns display file metadata and (for GET) a stream of the specified file in the content of the response.
        /// Returns a 201 created if the display file had to be created.
        /// <response>
        /// <status code="200">OK</status>
        /// <headers>
        /// <contentType>text/html</contentType>
        /// <contentDisposition>inline; filename=385.html</contentDisposition>
        /// <output>
        /// {
        ///     Id: 385,
        ///     Name: 33524,
        ///     Extension: "html"
        /// }
        /// </output>
        /// </headers>
        /// </response>
        /// </summary>
        [AcceptVerbs("GET", "HEAD")]
        public Task<HttpResponseMessage> GetDocumentRevisionFileStreamForDisplay(Int32 documentId, Int32 documentRevisionId, Int32 fileId)
        {
            return null;
        }
    }
}