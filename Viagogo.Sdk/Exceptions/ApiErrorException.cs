using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class ApiErrorException : ApiException
    {
        private readonly ApiError _error;

        public ApiErrorException(IApiResponse<ApiError> response) : base(response)
        {
            Requires.ArgumentNotNull(response, "response");

            _error = response.BodyAsObject;
        }

        public ApiError Error
        {
            get { return _error; }
        }
    }
}
