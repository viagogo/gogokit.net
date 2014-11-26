using System.Collections.Generic;
using System.Net;

namespace Viagogo.Sdk.Http
{
    public interface IApiResponse<T> : IApiResponse
    {
        T BodyAsObject { get; }
    }

    public interface IApiResponse
    {
        IDictionary<string, string> Headers { get; }
        HttpStatusCode StatusCode { get; }
        string Body { get; }
    }
}
