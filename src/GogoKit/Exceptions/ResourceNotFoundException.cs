using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ResourceNotFoundException : ApiErrorException
    {
        public ResourceNotFoundException(IApiResponse response) : base(response, null)
        {
        }

        public override string Message
        {
            get
            {
                return "The requested API resource was not found";
            }
        }
    }
}
