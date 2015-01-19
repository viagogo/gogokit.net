using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Configuration;
using GogoKit.Http;

namespace GogoKit.Tests.Fakes
{
    public class FakeHttpConnection : IHttpConnection
    {
        private readonly IApiResponse _response;

        public FakeHttpConnection(IApiResponse response = null, IConfiguration config = null)
        {
            _response = response;
            Configuration = config ?? GogoKit.Configuration.Configuration.Default;
        }

        public Task<IApiResponse<T>> SendRequestAsync<T>(Uri uri, HttpMethod method, string accept, object body, string contentType)
        {
            var response = _response as IApiResponse<T>;
            return Task.FromResult(response ?? new ApiResponse<T>());
        }

        public ICredentialsProvider CredentialsProvider { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
