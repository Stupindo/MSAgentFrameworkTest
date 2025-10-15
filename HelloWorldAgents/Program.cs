// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

string? githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

if (string.IsNullOrEmpty(githubToken))
{
    Console.WriteLine("Error: The GITHUB_TOKEN environment variable is not set.");
    Console.WriteLine("Please set the variable and restart your terminal/IDE.");
    return; // Exit the application
}

IChatClient chatClient =
    new ChatClient(
            "gpt-4o-mini",
            new ApiKeyCredential(githubToken),
            new OpenAIClientOptions { Endpoint = new Uri("https://models.github.ai/inference") })
        .AsIChatClient();

AIAgent writer = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "Writer",
        Instructions = "Write stories that are engaging and creative."
    });

AgentRunResponse response = await writer.RunAsync("Write a short story about a haunted house.");

Console.WriteLine(response.Text);
