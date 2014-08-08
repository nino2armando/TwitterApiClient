namespace TwitterApiClient.Core
{
    public interface IApiService
    {
        TwitterAuthorizationToken RequestToken();
    }
}