using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterApiClient.Core
{
    public class HttpClientService : IHttpClientService
    {
        public string Post(HttpParameters param)
        {
            if (string.IsNullOrEmpty(param.BaseUrl))
            {
                throw new ArgumentNullException("BaseUrl is required");
            }
            var client = new HttpClient{BaseAddress = new Uri(param.BaseUrl)};
            if (param.AuthorizationHeader != null)
            {
                client.DefaultRequestHeaders.Authorization = param.AuthorizationHeader;
            }
            if (param.DefaultHeaders.Any())
            {
                foreach (var header in param.DefaultHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            if (param.Content == null)
            {
                throw new InvalidOperationException("Http content most be provided");
            }

            HttpResponseMessage response;
            try
            {
                response = client.PostAsync(param.ResourceUrl, param.Content).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (TaskCanceledException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }

            string apiResponse;
            using (var responseStream = response.Content.ReadAsStreamAsync())
            using (var decompressedStream = new GZipStream(responseStream.Result, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                var rawResponse = streamReader.ReadToEnd();
                apiResponse = JsonConvert.DeserializeObject(rawResponse).ToString();
            }

            return apiResponse;
        }

        public string Get(HttpParameters param)
        {
            if (string.IsNullOrEmpty(param.BaseUrl))
            {
                throw new ArgumentNullException("BaseUrl is required");
            }
            var client = new HttpClient { BaseAddress = new Uri(param.BaseUrl) };

            if (param.AuthorizationHeader != null)
            {
                client.DefaultRequestHeaders.Authorization = param.AuthorizationHeader;
            }
            if (param.DefaultHeaders.Any())
            {
                foreach (var header in param.DefaultHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            string result;

            try
            {
                var requestTask = client.GetAsync(param.ResourceUrl);
                HttpResponseMessage apiResponse = requestTask.Result;
                apiResponse.EnsureSuccessStatusCode();

                using (var responseStream = apiResponse.Content.ReadAsStreamAsync())
                using (
                    var decompressedStream = new GZipStream(responseStream.Result,
                                                            CompressionMode.Decompress))
                using (var streamReader = new StreamReader(decompressedStream))
                {
                    var rawContent = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject(rawContent).ToString();
                }

            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (TaskCanceledException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }
    }
}
