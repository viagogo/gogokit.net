namespace Viagogo.Sdk.Authentication
{
    public interface ICredentials
    {
        string AuthorizationHeader { get; }
    }
}