using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InsufficientScopeException : ApiErrorException
    {
        public InsufficientScopeException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
