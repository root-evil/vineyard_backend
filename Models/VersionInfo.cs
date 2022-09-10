using System.Text.Json.Serialization;

namespace vineyard_backend.Models;

public class VersionInfo
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public DateTime Start { get; set; }

    [JsonIgnore]
    public string VersionString => Version ?? "undefined";
}