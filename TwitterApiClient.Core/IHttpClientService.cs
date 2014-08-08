namespace TwitterApiClient.Core
{
    public interface IHttpClientService
    {
        string Post(HttpParameters param);
        string Get(HttpParameters param);
    }
}