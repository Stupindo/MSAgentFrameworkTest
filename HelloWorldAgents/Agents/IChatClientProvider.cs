using Microsoft.Extensions.AI;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Creates chat clients for a specific provider.
/// </summary>
public interface IChatClientProvider
{
    /// <summary>
    /// Unique provider name used in configuration.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Creates a chat client for the supplied agent definition.
    /// </summary>
    /// <param name="definition">The agent configuration.</param>
    /// <param name="agentName">The logical name of the agent being created.</param>
    /// <returns>An <see cref="IChatClient"/> instance.</returns>
    IChatClient CreateChatClient(AgentDefinition definition, string agentName);
}
