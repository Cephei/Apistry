using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Apistry.Samples.Web.Controllers
{
    using Apistry.Samples.Web.Models;

    public class UserController : ApiController
    {
        public void Post([FromBody]User user)
        {
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Get(Int32 userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }


//        public void Put(Int32 userId, [FromBody]User user)
//        {
//        }

//        public void Delete(Int32 userId)
//        {
//        }
    }
}
