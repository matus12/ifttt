using System.Net.Http;

namespace Contracts.Wrappers
{
    public interface IHttpClientWrapper
    {
        HttpClient HttpClient { get; }
    }
}
