namespace HelloWorldAgents.Agents;

/// <summary>
/// Describes how an agent should be configured.
/// </summary>
public sealed class AgentDefinition
{
    /// <summary>
    /// Name of the provider used to build the chat client (e.g. "github" or "openai").
    /// </summary>
    public string Provider { get; set; } = "github";

    /// <summary>
    /// Model identifier to use when creating the chat client.
    /// </summary>
    public string Model { get; set; } = "gpt-4o-mini";

    /// <summary>
    /// Optional endpoint override for the provider.
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Optional name of the environment variable that stores the API key for the agent.
    /// </summary>
    public string? ApiKeyEnvironmentVariable { get; set; }

    /// <summary>
    /// Inline API key. Prefer <see cref="ApiKeyEnvironmentVariable"/> in production scenarios.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Natural language instructions that are passed to the agent.
    /// </summary>
    public string? Instructions { get; set; }
}
