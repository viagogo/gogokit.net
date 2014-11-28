namespace GogoKit.Authentication
{
    public interface ICredentials
    {
        string AuthorizationHeader { get; }
    }
}