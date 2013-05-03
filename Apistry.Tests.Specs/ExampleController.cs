namespace Apistry.Tests.Specs
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ExampleController : ApiController
    {
        public Task<HttpResponseMessage> CreateUser(Int32 siteId, User user, String role = null)
        {
            return Request.Content
                          .ReadAsStringAsync()
                          .ContinueWith(task => Request.CreateResponse(HttpStatusCode.Created));
        }
    }
}