using System;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(IApiResponse response)
        {
            Requires.ArgumentNotNull(response, nameof(response));

            Response = response;
        }

        public override string Message => $"An error occurred with this API request: {Response.Body}";

        public IApiResponse Response { get; }
    }
}
