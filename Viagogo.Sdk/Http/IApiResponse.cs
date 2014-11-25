using System.Collections.Generic;
using System.Net;

namespace Viagogo.Sdk.Http
{
    public interface IApiResponse<T>
    {
        IDictionary<string, string> Headers { get; }
        HttpStatusCode StatusCode { get; }
        string Body { get; }
        T BodyAsObject { get; }
    }
}
