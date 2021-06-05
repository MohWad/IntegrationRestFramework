using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace ConsoleClient
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();


        static async Task Main(string[] args)
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:5555");

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "IntegrationClient",
                ClientSecret = "123456",
                Scope = "getstudents"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(discoveryDocument.Error);
            }
            else
            {
                _httpClient.SetBearerToken(tokenResponse.AccessToken);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:6666/api/v1/students/all");
                //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(new { Pagination = new { PageNumber = 1, PageSize = 3 } }), Encoding.UTF8, "application/json");
                var studentResponse = await _httpClient.SendAsync(request);
                if (!studentResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine(studentResponse.StatusCode);
                }
                else
                {
                    var content = await studentResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }

        }
    }
}
