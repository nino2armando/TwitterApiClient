using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitterApiClient.Core
{
    public class HttpParameters
    {
        public AuthenticationHeaderValue AuthorizationHeader { get; set; }
        public StringContent Content { get; set; }
        public string BaseUrl { get; set; }
        public string ResourceUrl { get; set; }
        public Dictionary<string, string> DefaultHeaders { get; set; }
    }
}