namespace Apistry.Samples.Service.Api
{
    using System;
    using System.Net.Http;

    internal class ApiHttpMethod : HttpMethod
    {
        private static readonly HttpMethod _PatchMethod = new HttpMethod("PATCH");

        public ApiHttpMethod(String method) : base(method)
        {
        }

        public static HttpMethod Patch
        {
            get { return _PatchMethod; }
        }
    }
}