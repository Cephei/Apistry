// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRevisionFilesController.cs" company="Innovative Data Solutions">
//   Copyright © Innovative Data Solutions, Inc. 2012
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ids.Ecm.Service.Api.Policy
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Defines resources for document revision files.
    /// </summary>
    public class DocumentRevisionFilesController : ApiController
    {
        /// <summary>
        /// Takes an optional query string parameter "active" (true | false)
        /// Returns a list of DocumentRevisionFile Metadata
        /// If active is true it returns just the active file
        /// If active is false, it returns all but the active file
        /// If active is not set, it returns all files
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// <output>
        /// [{
        ///     Id: 33521,
        ///     SiteId: 1607, 
        ///     Name: 33521,
        ///     DisplayName: "385",
        ///     Extension: "html",
        ///     MD5Hash: "f8e73d61aa0afade5021434bf64a8136"
        /// }, {
        ///     Id: 33524,
        ///     SiteId: 1607,
        ///     Name: 33524,
        ///     DisplayName: "385",
        ///     Extension: "pdf",
        ///     MD5Hash: 81a20d3ae3db61b937c37c9fb875f892
        /// }] 
        /// </output>
        /// </response>
        [AcceptVerbs("PROPFIND")]
        public HttpResponseMessage GetDocumentRevisionFileMetadata(Int32 documentId, Int32 documentRevisionId, Boolean? active = null)
        {
            return null;
        }

        /// <summary>
        /// Uploads a file and associates it with the given Document/Revision
        /// </summary>
        /// <response>
        /// <status code="201">Created</status>
        /// <headers>
        /// <etag>"33524"</etag>
        /// <location>../powerdms/api/sites/current/documents/385/16746/files/33524</location>
        /// </headers>
        /// <output>
        /// {
        ///     DisplayName: "385",
        ///     Extension: "pdf",
        ///     MD5Hash: 81a20d3ae3db61b937c37c9fb875f892
        /// }
        /// </output>
        /// </response>
        public HttpResponseMessage PostDocumentRevisionFile(Int32 documentId, Int32 documentRevisionId, String fileName = null)
        {
            return null;
        }

        /// <summary>
        /// Locks the file to prevent other users from editing while an edit is already in progress
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// </response>
        [AcceptVerbs("LOCK")]
        public HttpResponseMessage LockDocumentRevisionFiles(Int32 documentId, Int32 documentRevisionId)
        {
            return null;          
        }

        /// <summary>
        /// Unlocks a file so that it can be edited. 
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// </response>
        [AcceptVerbs("UNLOCK")]
        public HttpResponseMessage UnlockDocumentRevisionFiles(Int32 documentId, Int32 documentRevisionId)
        {
            return null;
        }

        /// <summary>
        /// Returns file metadata and a stream of the specified file in the content of the response
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// <headers>
        /// <contentType>text/html</contentType>
        /// <contentDisposition>inline; filename=385.html</contentDisposition>
        /// </headers>
        /// <output>
        /// {
        ///     Id: 385,
        ///     Name: 33525,
        ///     Extension: "html",
        /// }
        /// </output>
        /// </response>
        public Task<HttpResponseMessage> GetDocumentRevisionFileStream(Int32 documentId, Int32 documentRevisionId, Int32 fileId)
        {
            return null;
        }
    }
}