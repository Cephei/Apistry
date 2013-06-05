namespace Apistry.Samples.Service.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Apistry.Samples.Application.Dto;
    using Apistry.Samples.Service.Api.Patching;

    /// <summary>
    /// This defines a crude representation of an un-documented ApiController.
    /// </summary>
    public class BlogsController : ApiController
    {
        public void DeleteBlog(Int32 blogId)
        {
            
        }

        public IEnumerable<BlogDto> GetBlogs()
        {
            return Enumerable.Empty<BlogDto>();
        }

        public BlogDto GetBlog(Int32 blogId)
        {
            return null;
        }

        public HttpResponseMessage PatchBlog(PatchRequest<BlogDto> patchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage PostBlog(BlogDto blog)
        {
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}