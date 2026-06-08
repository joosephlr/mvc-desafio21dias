using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace mvc.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _supabaseUrl;
        private readonly string _apiKey;

        public SupabaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _supabaseUrl = configuration["Supabase:Url"];
            _apiKey = configuration["Supabase:ApiKey"];
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint)
        {
            var url = $"{_supabaseUrl}/rest/v1/{endpoint}";
            var request = new HttpRequestMessage(method, url);
            
            // Adicionar headers de autenticação
            request.Headers.Add("apikey", _apiKey);
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Headers.Add("Prefer", "return=representation");
            
            return request;
        }

        public async Task<string> GetAsJsonAsync(string endpoint)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                    throw new Exception("API Key não configurada! Verifique appsettings.json");

                var request = CreateRequest(HttpMethod.Get, endpoint);
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na requisição: {response.StatusCode} - {errorContent}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar dados: {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                    throw new Exception("API Key não configurada! Verifique appsettings.json");

                var request = CreateRequest(HttpMethod.Get, endpoint);
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro na requisição: {response.StatusCode} - {errorContent}");
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GET Response: {json.Substring(0, Math.Min(100, json.Length))}...");
                
                var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar dados: {ex.Message}");
                throw;
            }
        }

        public async Task<string> PostAsync(string endpoint, object data)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                    throw new Exception("API Key não configurada! Verifique appsettings.json");

                var request = CreateRequest(HttpMethod.Post, endpoint);
                var json = JsonSerializer.Serialize(data);
                
                Console.WriteLine($"POST URL: {request.RequestUri}");
                Console.WriteLine($"POST Data: {json}");
                Console.WriteLine($"API Key (primeiros 20 chars): {_apiKey.Substring(0, Math.Min(20, _apiKey.Length))}...");
                
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"POST Response Status: {response.StatusCode}");
                Console.WriteLine($"POST Response Content: {responseContent}");
                
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erro na requisição: {response.StatusCode} - {responseContent}");

                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir dados: {ex.Message}");
                throw;
            }
        }

        public async Task<string> PatchAsync(string endpoint, object data)
        {
            try
            {
                var request = CreateRequest(HttpMethod.Patch, endpoint);
                var json = JsonSerializer.Serialize(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erro na requisição: {response.StatusCode}");

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar dados: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(string endpoint)
        {
            try
            {
                var request = CreateRequest(HttpMethod.Delete, endpoint);
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erro na requisição: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar dados: {ex.Message}");
                throw;
            }
        }
    }
}
