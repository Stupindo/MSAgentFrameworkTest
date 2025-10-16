using System.ClientModel;

namespace HelloWorldAgents.Agents;

/// <summary>
/// Base class for providers that rely on API keys stored in environment variables.
/// </summary>
public abstract class ApiKeyChatClientProvider : IChatClientProvider
{
    public abstract string Name { get; }

    public abstract Microsoft.Extensions.AI.IChatClient CreateChatClient(AgentDefinition definition, string agentName);

    /// <summary>
    /// Resolves the API key from inline configuration or environment variables.
    /// </summary>
    /// <param name="definition">The agent definition to inspect.</param>
    /// <param name="agentName">Name of the agent, used for error messages.</param>
    /// <param name="fallbackEnvironmentVariable">Environment variable checked when no custom one is provided.</param>
    /// <returns>The API key.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no key can be resolved.</exception>
    protected static ApiKeyCredential ResolveApiKeyCredential(
        AgentDefinition definition,
        string agentName,
        string fallbackEnvironmentVariable)
    {
        string? apiKey = definition.ApiKey;

        if (string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(definition.ApiKeyEnvironmentVariable))
        {
            apiKey = Environment.GetEnvironmentVariable(definition.ApiKeyEnvironmentVariable);
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            apiKey = Environment.GetEnvironmentVariable(fallbackEnvironmentVariable);
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            string variableName = definition.ApiKeyEnvironmentVariable ?? fallbackEnvironmentVariable;
            throw new InvalidOperationException(
                $"No API key was found for agent '{agentName}'. Set the '{variableName}' environment variable or configure an inline key.");
        }

        return new ApiKeyCredential(apiKey);
    }

    /// <summary>
    /// Normalizes an endpoint string and converts it to a <see cref="Uri"/>.
    /// </summary>
    /// <param name="endpoint">Endpoint from the configuration.</param>
    /// <param name="defaultEndpoint">Endpoint used when none is specified.</param>
    /// <param name="providerName">Name of the provider for error reporting.</param>
    /// <returns>The URI to use when constructing the client.</returns>
    protected static Uri ResolveEndpoint(string? endpoint, Uri defaultEndpoint, string providerName)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return defaultEndpoint;
        }

        if (!Uri.TryCreate(endpoint, UriKind.Absolute, out Uri? uri))
        {
            throw new InvalidOperationException(
                $"The endpoint '{endpoint}' is not a valid absolute URI for provider '{providerName}'.");
        }

        return uri;
    }
}
