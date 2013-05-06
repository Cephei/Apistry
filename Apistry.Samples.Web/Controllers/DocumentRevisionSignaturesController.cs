// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRevisionSignaturesController.cs" company="Innovative Data Solutions">
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
    
    /// <summary>
    /// Defines resources for Document Revisions Signatures
    /// </summary>
    public class DocumentRevisionSignaturesController : ApiController 
    {
        /// <summary>
        /// Gets a signature if the user has audit rights over the document or if it's the current user's signature 
        /// </summary>
        /// <response>
        /// <status code="200">OK</status>
        /// <output>
        /// {
        ///     Id: 76985,
        ///     DocumentRevisionId: 35742,
        ///     UserId: 321,
        ///     FileId: 385,
        ///     DisplayFileId: 385,
        ///     ConfirmationToken: ,
        ///     Signed: true
        /// }
        /// </output>
        /// </response>
        public HttpResponseMessage GetDocumentRevisionSignature(Int32 documentId, Int32 documentRevisionId, Int32 signatureId)
        {
            return null;
        }
    }
}