using HelloWorldAgents.Agents;
using Microsoft.Agents.AI;

string? configurationPath = Environment.GetEnvironmentVariable("AGENT_CONFIG_PATH");
configurationPath = string.IsNullOrWhiteSpace(configurationPath)
    ? Path.Combine(AppContext.BaseDirectory, "agents.json")
    : configurationPath;

AgentConfiguration configuration;
try
{
    configuration = AgentConfigurationLoader.Load(configurationPath);
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to load agent configuration from '{configurationPath}': {ex.Message}");
    return;
}

if (configuration.Agents.Count == 0)
{
    Console.WriteLine("No agents are defined in the configuration.");
    return;
}

var agentFactory = AgentFactory.CreateDefault();
var agents = new Dictionary<string, AIAgent>(StringComparer.OrdinalIgnoreCase);

foreach ((string name, AgentDefinition definition) in configuration.Agents)
{
    try
    {
        agents[name] = agentFactory.CreateAgent(name, definition);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to initialize agent '{name}': {ex.Message}");
    }
}

if (!agents.TryGetValue(configuration.PrimaryAgent, out AIAgent? primaryAgent))
{
    Console.WriteLine(
        $"The configured primary agent '{configuration.PrimaryAgent}' could not be created. Check the provider and API key settings.");
    return;
}

string prompt = args.Length > 0 ? string.Join(' ', args) : configuration.InitialPrompt;
if (string.IsNullOrWhiteSpace(prompt))
{
    Console.WriteLine("No prompt was supplied. Provide one via the command line or update the configuration file.");
    return;
}

configuration.Agents.TryGetValue(configuration.PrimaryAgent, out AgentDefinition? primaryDefinition);
string primaryProvider = primaryDefinition?.Provider ?? "unknown";

Console.WriteLine($"Running agent '{configuration.PrimaryAgent}' using provider '{primaryProvider}'.");

AgentRunResponse response;
try
{
    response = await primaryAgent.RunAsync(prompt);
}
catch (Exception ex)
{
    Console.WriteLine($"The primary agent run failed: {ex.Message}");
    return;
}

string currentOutput = response.Text ?? string.Empty;
Console.WriteLine($"\n[{configuration.PrimaryAgent}]\n{currentOutput}\n");

if (configuration.Injections.Count > 0)
{
    foreach (AgentInjection injection in configuration.Injections)
    {
        if (!agents.TryGetValue(injection.Agent, out AIAgent? agent))
        {
            Console.WriteLine($"Skipping injection for unknown agent '{injection.Agent}'.");
            continue;
        }

        string injectionPrompt = injection.BuildPrompt(currentOutput);

        configuration.Agents.TryGetValue(injection.Agent, out AgentDefinition? injectionDefinition);
        string injectionProvider = injectionDefinition?.Provider ?? "unknown";

        Console.WriteLine($"Running agent '{injection.Agent}' using provider '{injectionProvider}'.");

        AgentRunResponse injectionResponse;
        try
        {
            injectionResponse = await agent.RunAsync(injectionPrompt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Agent '{injection.Agent}' failed: {ex.Message}");
            continue;
        }

        currentOutput = injectionResponse.Text ?? string.Empty;
        Console.WriteLine($"\n[{injection.Agent}]\n{currentOutput}\n");
    }
}
