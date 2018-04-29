using System.Net.Http;
using Contracts.Wrappers;

namespace ContentService.Wrappers
{
    internal class HttpClientWrapper : IHttpClientWrapper
    {
        public HttpClientWrapper()
        {
            HttpClient = new HttpClient();
        }

        public HttpClient HttpClient { get; }
    }
}
