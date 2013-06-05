namespace Apistry.Samples.Service.Api.Configuration
{
    using System.Web.Http;
    using NContext.Extensions.AspNetWebApi.Configuration;

    internal class WebApiConfiguration : IConfigureWebApi
    {
        public void Configure(HttpConfiguration configuration)
        {
        }
    }
}