using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace TwitterApiClient.Core
{
    public class ApiService : IApiService
    {
        readonly string _baseUrl = ConfigurationManager.AppSettings["api_base_url"];
        readonly string _oauthConsumerKey = ConfigurationManager.AppSettings["oauth_consumer_key"];
        readonly string _oauthConsumerSecret = ConfigurationManager.AppSettings["oauth_consumer_secret"];
        private readonly IHttpClientService _httpClient;
        public const string TokenType = "bearer";

        public ApiService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public TwitterAuthorizationToken RequestToken()
        {
            const string grantType = "grant_type=client_credentials";

            var content = new StringContent(grantType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            var param = new HttpParameters
            {
                BaseUrl = _baseUrl,
                AuthorizationHeader = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", Uri.EscapeDataString(_oauthConsumerKey),
                                  Uri.EscapeDataString(_oauthConsumerSecret))))),
                Content = content,
                DefaultHeaders = new Dictionary<string, string> { { "Accept-Encoding", "gzip" }, { "Host", "api.twitter.com" } },
                ResourceUrl = "oauth2/token"
            };


            var rawJWt = _httpClient.Post(param);
            var   token = JsonConvert.DeserializeObject<TwitterAuthorizationToken>(rawJWt);

            if (!token.TokenType.Equals(TokenType, StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            return token;
        }
    }
}
