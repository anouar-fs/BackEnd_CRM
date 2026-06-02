using BackEnd.Dto;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class TypesenseService
{
    private readonly HttpClient _httpClient;

    public TypesenseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateLeadsCollection()
    {
        var request1 = new HttpRequestMessage(HttpMethod.Delete, "/collections/leads");
        request1.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response1 = await _httpClient.SendAsync(request1);

        var payload = new
        {
            name = "leads",
            fields = new[]
            {
                new { name = "id", type = "string", infix = true },
                new { name = "firstName", type = "string", infix = true },
                new { name = "lastName", type = "string", infix = true  },
                new { name = "email", type = "string", infix = true  },
                new { name = "phone", type = "string", infix = true }
            }
        };

        var json = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post, "/collections")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response = await _httpClient.SendAsync(request);

        var body = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
    }

    public async Task IndexLeadAsync(LeadIndexDto lead)
    {
        var payload = JsonSerializer.Serialize(lead);

        var request = new HttpRequestMessage(HttpMethod.Post, "/collections/leads/documents")
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response = await _httpClient.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateLeadAsync(LeadDto lead)
    {
        var payload = JsonSerializer.Serialize(lead);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/collections/leads/documents/{lead.Id}")
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteLeadAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/collections/leads/documents/{id}");

        request.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<string> SearchLeads(string query)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"/collections/leads/documents/search?q={query}&query_by=firstName,lastName,email,phone&infix=off,off,off,always");

        request.Headers.Add("X-TYPESENSE-API-KEY", "xyz");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

}