using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.Extensions.Configuration;


namespace kajiapp.Infra.Services
{
    // Infra/Service/DiscordNotifier.cs
    public class DiscordNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookUrl;

        public DiscordNotifier(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _httpClient = httpClient;
            _webhookUrl = config["Discord:WebhookUrl"]; // appsettingsに定義
        }

        public async Task SendAsync(string message)
        {
            var payload = new { content = message };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_webhookUrl, content);
        }
    }

}
