using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Creates <see cref="AIAgent"/> instances from <see cref="AgentDefinition"/> objects.
/// </summary>
public sealed class AgentFactory
{
    private readonly Dictionary<string, IChatClientProvider> _providers;

    public AgentFactory(IEnumerable<IChatClientProvider> providers)
    {
        _providers = providers.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }

    public static AgentFactory CreateDefault() =>
        new(new IChatClientProvider[]
        {
            new GitHubChatClientProvider(),
            new OpenAIChatClientProvider()
        });

    /// <summary>
    /// Creates an <see cref="AIAgent"/> using the provided definition.
    /// </summary>
    /// <param name="name">Logical name of the agent.</param>
    /// <param name="definition">Configuration settings for the agent.</param>
    /// <returns>The configured agent.</returns>
    public AIAgent CreateAgent(string name, AgentDefinition definition)
    {
        if (!_providers.TryGetValue(definition.Provider, out IChatClientProvider? provider))
        {
            throw new InvalidOperationException(
                $"Unknown provider '{definition.Provider}' for agent '{name}'.");
        }

        IChatClient chatClient = provider.CreateChatClient(definition, name);
        var options = new ChatClientAgentOptions
        {
            Name = name,
            Instructions = definition.Instructions
        };

        return new ChatClientAgent(chatClient, options);
    }
}
