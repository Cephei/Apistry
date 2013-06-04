
namespace Apistry.Samples.Service.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Apistry.Samples.Application.Dto;

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
}