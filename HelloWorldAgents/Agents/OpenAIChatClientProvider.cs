using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Creates chat clients that communicate with OpenAI hosted models.
/// </summary>
public sealed class OpenAIChatClientProvider : ApiKeyChatClientProvider
{
    private static readonly Uri DefaultEndpoint = new("https://api.openai.com/v1");

    public override string Name => "openai";

    public override IChatClient CreateChatClient(AgentDefinition definition, string agentName)
    {
        string model = string.IsNullOrWhiteSpace(definition.Model) ? "gpt-4o-mini" : definition.Model;
        Uri endpoint = ResolveEndpoint(definition.Endpoint, DefaultEndpoint, Name);
        var credential = ResolveApiKeyCredential(definition, agentName, "OPENAI_API_KEY");

        var chatClient = new ChatClient(model, credential, new OpenAIClientOptions { Endpoint = endpoint });
        return chatClient.AsIChatClient();
    }
}
