using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Almacen._2.Common.Models.System;

namespace Api._3.Services
{
    public class ApiService<T>
    {
        private readonly HttpClient httpClient;
        private readonly string baseUri;

        public ApiService(string baseUri)
        {
            this.baseUri = baseUri;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync(string endpoint, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);
            var response = await httpClient.GetAsync($"{baseUri}/{endpoint}");
            
            if (!response.IsSuccessStatusCode)
                throw new ServicesException(Constants.ErrorServicioExterno, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse)!;
        }

        public async Task<byte[]> GetFileAsync(string endpoint, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);
            var response = await httpClient.GetAsync($"{baseUri}/{endpoint}");

            if (!response.IsSuccessStatusCode)
                throw new ServicesException(Constants.ErrorServicioExterno, (int)response.StatusCode);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string accessToken = "")
        {
            //comento esto
            if (!string.IsNullOrWhiteSpace(accessToken))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);
            
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{baseUri}/{endpoint}/", content);

            if (!response.IsSuccessStatusCode)
                throw new ServicesException(Constants.ErrorServicioExterno, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(jsonResponse)!;
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var requestUri = $"{baseUri}/{endpoint}";
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(jsonResponse);
            }

            throw new ServicesException(Constants.ErrorServicioExterno, (int)response.StatusCode);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string endpoint, string accessToken = "")
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);

            var response = await httpClient.DeleteAsync($"{baseUri}/{endpoint}/");

            if (!response.IsSuccessStatusCode)
                throw new ServicesException(Constants.ErrorServicioExterno, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(jsonResponse)!;
        }
    }
}
