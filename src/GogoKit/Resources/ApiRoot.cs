using GogoKit.Json;

namespace GogoKit.Resources
{
    public class ApiRoot : Resource
    {
        [Embedded("user")]
        public User AuthenticatedUser { get; set; }
    }
}
