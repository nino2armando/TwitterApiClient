using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TwitterApiClient.Core.Test
{
    [TestClass]
    public class HttpClientServiceTest
    {
        readonly string _baseUrl = ConfigurationManager.AppSettings["api_base_url"];
        readonly string _oauthConsumerKey = ConfigurationManager.AppSettings["oauth_consumer_key"];
        readonly string _oauthConsumerSecret = ConfigurationManager.AppSettings["oauth_consumer_secret"];

        [TestMethod]
        public void PostCredentials_ApiCallSuccessful_ReturnToken()
        {
            const string GRANT_TYPE = "grant_type=client_credentials";
            var client = new HttpClientService();

            var content = new StringContent(GRANT_TYPE);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            var postParam = new HttpParameters
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
            var result = client.Post(postParam);


            var token = JsonConvert.DeserializeObject<TwitterAuthorizationToken>(result);

            var getParam = new HttpParameters
            {
                BaseUrl = _baseUrl,
                DefaultHeaders = new Dictionary<string, string> { { "Accept-Encoding", "gzip" },  {"Authorization",
                                                                       string.Format("{0} {1}",
                                                                                     token.TokenType,
                                                                                     token.AccessToken)}},
                ResourceUrl = "1.1/search/tweets.json?q=%23freebandnames&since_id=24012619984051000&max_id=250126199840518145&result_type=mixed&count=4"
            };

            var payloade = client.Get(getParam);
        }
    }
}
