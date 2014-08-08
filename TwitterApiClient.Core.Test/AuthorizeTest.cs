using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace TwitterApiClient.Core.Test
{
    [TestClass]
    public class AuthorizeTest
    {
        private Mock<IHttpClientService> _clientService;
        private IApiService _apiService;

        [TestInitialize]
        public void Setup()
        {
            _clientService = new Mock<IHttpClientService>();
            _apiService = new ApiService(_clientService.Object);
        }


        [TestMethod]
        public void RequestToken_TokenTypeBearer_ShouldReturnValidToken()
        {
            var token = new TwitterAuthorizationToken
            {
                TokenType = "bearer",
                AccessToken = "AAAAAAAAAAAAAAABBBBBBBBBBBBCCCCCCCCCCCCCC"
            };

            var serialized = JsonConvert.SerializeObject(token);

            _clientService.Setup(a => a.Post(It.IsAny<HttpParameters>())).Returns(serialized);

            var apiToken = _apiService.RequestToken();

            Assert.IsNotNull(apiToken);
        }

        [TestMethod]
        public void RequestToken_DoesNotTokenContainsBearerTokenType_ShouldNotReturnToken()
        {
            var token = new TwitterAuthorizationToken
                {
                    TokenType = "bearers",
                    AccessToken = "AAAAAAAAAAAAAAABBBBBBBBBBBBCCCCCCCCCCCCCC"
                };

            var serialized = JsonConvert.SerializeObject(token);

            _clientService.Setup(a => a.Post(It.IsAny<HttpParameters>())).Returns(serialized);

            var apiToken = _apiService.RequestToken();

            Assert.IsNull(apiToken);
        }
    }
}
