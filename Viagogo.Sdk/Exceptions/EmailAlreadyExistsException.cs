using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class EmailAlreadyExistsException : ApiErrorException
    {
        public EmailAlreadyExistsException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}