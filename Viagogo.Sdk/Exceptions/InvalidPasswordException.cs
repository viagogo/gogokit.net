using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InvalidPasswordException : ApiErrorException
    {
        public InvalidPasswordException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}