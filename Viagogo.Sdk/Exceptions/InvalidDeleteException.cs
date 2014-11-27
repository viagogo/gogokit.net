using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InvalidDeleteException : ApiErrorException
    {
        public InvalidDeleteException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}