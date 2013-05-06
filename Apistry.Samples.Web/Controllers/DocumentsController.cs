// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentsController.cs" company="Innovative Data Solutions">
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
    /// Defines resources for documents.
    /// </summary>
    public class DocumentsController : ApiController
    {
        /// <summary>
        /// Returns a Document if the user meets any of the following conditions: <br/>
        /// 1) The user has explicit view rights   <br/>
        /// 2) The user is assigned to the current publication  <br/>
        /// 3) The user has signed a previous revision <br/>
        /// </summary>
        /// <param name="documentId"></param>
        /// <response>
        /// <status code="200">OK</status>
        /// <output>
        /// {
        ///     "Id": 385,
        ///     "Name": "The Best Document",
        ///     "Description": "The best document in the world",
        ///     "Permissions": "View",
        ///     "EffectiveOn": "2013-03-26T04:00:00.000Z",
        ///     "IsAssigned": false 
        /// }    
        /// </output>
        /// </response>
        public HttpResponseMessage GetDocument(Int32 documentId)
        {
            return null;
        }

        /// <summary>
        /// Changes document metadata if the user has Edit rights
        /// </summary>
        /// <inputs>
        /// <input name="Name" required="false" type="String">The name of the document</input>
        /// <input name="Description" required="false" type="String">The description of the document</input>
        /// <input name="EffectiveOn" required="false" type="String">String representing the effective date for the document in JSON format</input>
        /// </inputs>
        /// <param name="documentId"></param>
        /// <param name="documentPatch"></param>
        /// <response>
        /// <status code="204">No Content</status>
        /// </response>
        public HttpResponseMessage PatchDocument(Int32 documentId, PatchRequest documentPatch)
        {
            return null;
        }

        /// <summary>
        /// Deletes a document and all its revisions and history if the user has Edit rights
        /// </summary>
        /// <param name="documentId"></param>
        /// <response>
        /// <status code="204">No Content</status>
        /// </response>
        public HttpResponseMessage DeleteDocument(Int32 documentId)
        {
            return null;
        }
    }
}