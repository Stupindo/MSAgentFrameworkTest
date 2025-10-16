namespace HelloWorldAgents.Agents;

/// <summary>
/// Represents the configuration used to build one or more agents.
/// </summary>
public sealed class AgentConfiguration
{
    /// <summary>
    /// Name of the primary agent to execute.
    /// </summary>
    public string PrimaryAgent { get; set; } = "writer";

    /// <summary>
    /// Default prompt used when none is supplied on the command line.
    /// </summary>
    public string InitialPrompt { get; set; } = "Write a short story about a haunted house.";

    /// <summary>
    /// Agents defined in the configuration. Keys correspond to the logical agent names.
    /// </summary>
    public Dictionary<string, AgentDefinition> Agents { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["writer"] = new AgentDefinition
        {
            Provider = "github",
            Model = "gpt-4o-mini",
            ApiKeyEnvironmentVariable = "GITHUB_TOKEN",
            Instructions = "Write stories that are engaging and creative."
        }
    };

    /// <summary>
    /// Additional agents that should be invoked after the primary agent completes.
    /// </summary>
    public List<AgentInjection> Injections { get; set; } = new();
}
