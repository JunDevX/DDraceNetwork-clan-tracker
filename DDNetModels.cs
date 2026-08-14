using System.Text.Json.Serialization;

namespace DDNetTracker;

public class DDNetResponse
{
    [JsonPropertyName("servers")]
    public List<Server>? Servers { get; set; }
}

public class Server
{
    [JsonPropertyName("info")]
    public ServerInfo? Info { get; set; }
}

public class ServerInfo
{
    [JsonPropertyName("name")]
    public string ServerName { get; set; } = string.Empty;

    [JsonPropertyName("clients")]
    public List<Client>? Clients { get; set; }
}

public class Client
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("clan")]
    public string Clan { get; set; } = string.Empty;
}

public class PlayerDisplayModel
{
    public string Name { get; set; } = string.Empty;
    public string Clan { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
}