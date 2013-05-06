// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRevisionsController.cs" company="Innovative Data Solutions">
//   Copyright © 2013 Innovative Data Solutions, Inc.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ids.Ecm.Service.Api.Policy
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using Apistry.Samples.Web.Models;

    /// <summary>
    /// Defines resources for document revisions.
    /// </summary>
    public class DocumentRevisionsController : ApiController
    {
        /// <summary>
        /// Returns a list of revisions metadata where each revision meets the following criteria:
        /// 1) The user has Audit rights
        /// 2) Or the Document is enabled and
        ///     a) And the user has signed this revision
        ///     b) Or the revision is published
        ///         i) And the user is assigned
        ///         ii) Or the user has view rights
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// <output>
        /// [{
        ///     Id: 23,
        ///     FileId: 675,
        ///     FileExtension: "Docx",
        ///     Status: "Published",
        ///     RequiredMinutesToSign: 0,
        ///     RequiredTestToSign: 5870,
        ///     ShouldSign: true,
        /// }, {
        ///     Id: 16308,
        ///     FileId: 31864,
        ///     FileExtension: "Html",
        ///     Status: "Archived",
        ///     RequiredMinutesToSign: 0,
        ///     ShouldSign: false,
        ///     Signed: "2012-05-25T15:04:22.25-04:00"
        /// }, {
        ///     Id: 16747,
        ///     FileId: 33523,
        ///     FileExtension: "Pdf"
        ///     Status: "Draft",
        ///     RequiredMinutesToSign: 5,
        ///     ShouldSign: false,
        /// }]
        /// </output>
        /// </response>
        // Todo(AD): This should probably a GET
        [AcceptVerbs("PROPFIND")]
        public HttpResponseMessage PropFindDocumentRevisionsMetadata(Int32 documentId)
        {
            return null;
        }

        /// <summary>
        /// Updates a Document Revision
        /// </summary>
        /// <inputs>
        /// <input name="Status" type="String" sample="200" required="false">The lifecycle status of a revision (Draft, published, or Archived)</input>
        /// <input name="RequiredMinutesToSign" type="Int32" required="false">The amount of time in minutes a user must read a document before they can sign</input>
        /// </inputs>
        public HttpResponseMessage PatchDocument(Int32 documentId, Int32 documentRevisionId, PatchRequest documentPatch)
        {
            return null;
        }
    }
}