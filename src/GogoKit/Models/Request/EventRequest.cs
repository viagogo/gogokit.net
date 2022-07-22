namespace GogoKit.Models.Request
{
    public class EventRequest : RequestParameters<EventEmbed,EventSort>
    {
    }

    public enum EventEmbed
    {
    }

    public enum EventSort
    {
        ResourceVersion
    }
}
