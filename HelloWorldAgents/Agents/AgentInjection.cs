namespace HelloWorldAgents.Agents;

/// <summary>
/// Describes how an additional agent should be invoked after the primary agent completes.
/// </summary>
public sealed class AgentInjection
{
    /// <summary>
    /// Name of the agent that should be invoked.
    /// </summary>
    public string Agent { get; set; } = string.Empty;

    /// <summary>
    /// Prompt template passed to the agent. The token "{output}" is replaced with the
    /// primary agent response.
    /// </summary>
    public string PromptTemplate { get; set; } = "Review the following output and provide feedback:\n{output}";

    /// <summary>
    /// Builds the prompt that will be sent to the agent.
    /// </summary>
    /// <param name="output">The previous output.</param>
    /// <returns>The prompt string.</returns>
    public string BuildPrompt(string output)
    {
        if (string.IsNullOrEmpty(PromptTemplate))
        {
            return output;
        }

        return PromptTemplate.Replace("{output}", output, StringComparison.OrdinalIgnoreCase);
    }
}
