using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Creates chat clients that communicate with GitHub Models.
/// </summary>
public sealed class GitHubChatClientProvider : ApiKeyChatClientProvider
{
    private static readonly Uri DefaultEndpoint = new("https://models.github.ai/inference");

    public override string Name => "github";

    public override IChatClient CreateChatClient(AgentDefinition definition, string agentName)
    {
        string model = string.IsNullOrWhiteSpace(definition.Model) ? "gpt-4o-mini" : definition.Model;
        Uri endpoint = ResolveEndpoint(definition.Endpoint, DefaultEndpoint, Name);
        var credential = ResolveApiKeyCredential(definition, agentName, "GITHUB_TOKEN");

        var chatClient = new ChatClient(model, credential, new OpenAIClientOptions { Endpoint = endpoint });
        return chatClient.AsIChatClient();
    }
}
