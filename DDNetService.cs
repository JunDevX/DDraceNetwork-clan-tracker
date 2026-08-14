using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDNetTracker;

public class DDNetService
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiUrl = "https://master1.ddnet.org/ddnet/15/servers.json";

    public async Task<List<PlayerDisplayModel>> GetOnlinePlayersAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(ApiUrl);
            var data = JsonSerializer.Deserialize<DDNetResponse>(response);

            var players = new List<PlayerDisplayModel>();

            if (data?.Servers == null) return players;

            foreach (var server in data.Servers)
            {
                if (server.Info?.Clients == null) continue;

                foreach (var client in server.Info.Clients)
                {
                    if (!string.IsNullOrWhiteSpace(client.Name))
                    {
                        players.Add(new PlayerDisplayModel
                        {
                            Name = client.Name,
                            Clan = string.IsNullOrWhiteSpace(client.Clan) ? "Без клана" : client.Clan,
                            Server = server.Info.ServerName
                        });
                    }
                }
            }

            return players;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return new List<PlayerDisplayModel>();
        }
    }
}