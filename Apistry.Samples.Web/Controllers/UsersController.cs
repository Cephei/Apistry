
namespace Apistry.Samples.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Apistry.Samples.Web.Models;
    
    /// <summary>
    /// Defines a crude representation of a well documented ApiController.
    /// </summary>
    public class UsersController : ApiController
    {
        public HttpResponseMessage DeleteUser(Int32 userId)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage GetUsers()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetUser(Int32 userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PatchUser(Int32 userId, PatchRequest<UserDto> patchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage PostUser(UserDto userDto)
        {
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public HttpResponseMessage PutUser(Int32 userId, UserDto userDto)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }


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

        public HttpResponseMessage PutBlog(Int32 blogId, BlogDto blog)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}