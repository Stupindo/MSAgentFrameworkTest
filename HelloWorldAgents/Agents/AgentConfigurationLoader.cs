using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Loads <see cref="AgentConfiguration"/> instances from disk.
/// </summary>
public static class AgentConfigurationLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    /// <summary>
    /// Loads configuration from the specified path. When the file does not exist, a default
    /// configuration is returned.
    /// </summary>
    /// <param name="path">The configuration file path.</param>
    /// <returns>The deserialized configuration.</returns>
    public static AgentConfiguration Load(string path)
    {
        AgentConfiguration configuration;

        if (File.Exists(path))
        {
            using FileStream stream = File.OpenRead(path);
            configuration = JsonSerializer.Deserialize<AgentConfiguration>(stream, SerializerOptions)
                ?? new AgentConfiguration();
        }
        else
        {
            configuration = new AgentConfiguration();
        }

        // Ensure dictionaries use case-insensitive lookups.
        configuration.Agents = new Dictionary<string, AgentDefinition>(configuration.Agents, StringComparer.OrdinalIgnoreCase);

        return configuration;
    }
}
