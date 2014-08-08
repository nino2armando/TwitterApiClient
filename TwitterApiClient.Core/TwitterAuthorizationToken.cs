﻿using Newtonsoft.Json;

namespace TwitterApiClient.Core
{
    public class TwitterAuthorizationToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
