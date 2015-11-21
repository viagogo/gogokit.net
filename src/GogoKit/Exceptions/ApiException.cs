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

        public override string Message
        {
            get
            {
                string message = null;
                var authorizationErrorResponse = Response as IApiResponse<AuthorizationError>;
                if (authorizationErrorResponse != null &&
                    authorizationErrorResponse.BodyAsObject != null)
                {
                    message = authorizationErrorResponse.BodyAsObject.ErrorDescription;
                }

                return message ?? "An error occurred with this API request";
            }
        }

        public IApiResponse Response { get; }
    }
}
