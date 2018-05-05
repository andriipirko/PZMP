using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdminAccountingApp
{
    class RestService
    {
        HttpClient _client;
        public RestService()
        {
            _client = new HttpClient();
            _client.MaxResponseContentBufferSize = 256000;
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded' "));
        }

        public async Task<List<Customers>> GetCustomersAsync()
        {
            List<Customers> result = null;

            string url = "http://192.168.1.20:5999/api/Customers/GetAll";

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<Customers>>(content);

            }
            catch { return null; }


            return result;
        }

        public async Task<bool> StartServer(int id)
        {
            bool result = false;
            string url = "http://192.168.1.20:5999/api/Customers/Start/" + id;

            try
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(id));
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url);
                request.Method = HttpMethod.Post;
                request.Content = content;
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            }
            catch { return false; }

            return result;
        }

        public async Task<bool> StopServer(int port)
        {
            bool result = true;
            string url = $"http://192.168.1.20:{port}/api/Config/Exit";

            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url);
                request.Method = HttpMethod.Post;
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

            }
            catch { return false; }

            return result;
        }

        public async Task<bool> ServerWasStarted(int port)
        {
            string url = $"http://192.168.1.20:{port}/api/Config/CheckAccessibility";
            bool result = true;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
            }
            catch { return false; }


            return result;
        }
    }
}
