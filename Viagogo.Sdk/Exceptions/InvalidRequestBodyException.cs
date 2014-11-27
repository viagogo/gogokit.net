using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InvalidRequestBodyException : ApiErrorException
    {
        public InvalidRequestBodyException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}