using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Http;

namespace GogoKit.Tests.Fakes
{
    public class FakeConnection : IConnection
    {
        private readonly IApiResponse _response;

        public FakeConnection(IApiResponse response = null)
        {
            _response = response;
        }

        public Task<IApiResponse<T>> SendRequestAsync<T>(Uri uri, HttpMethod method, string accept, object body, string contentType)
        {
            var response = _response as IApiResponse<T>;
            return Task.FromResult(response ?? new ApiResponse<T>());
        }

        public ICredentialsProvider CredentialsProvider { get; set; }
    }
}
