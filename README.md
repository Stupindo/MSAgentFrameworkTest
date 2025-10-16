# MSAgentFrameworkTest

This sample demonstrates how to build composable agents using the Microsoft Agents framework.

## Prerequisites

* .NET 9.0 SDK
* API keys for the providers you plan to use (for example GitHub Models or OpenAI)

## Configuration

Agents are configured through `HelloWorldAgents/agents.json`. The file is copied to the build output directory so it can be customised without recompiling. Each agent definition specifies:

* `provider` – `github` or `openai` out of the box.
* `model` – model identifier to target.
* `apiKeyEnvironmentVariable` or `apiKey` – how the credential is supplied.
* `instructions` – guidance sent to the agent at run time.

The configuration also allows you to inject additional agents via the `injections` array. Each injection references an agent by name and supplies a `promptTemplate`. The placeholder `{output}` is replaced with the previous agent's response before invoking the next agent.

Set `AGENT_CONFIG_PATH` to load a different configuration file at runtime.

## Running the sample

Restore packages and run the application:

```bash
cd HelloWorldAgents
dotnet run
```

Provide a prompt via the command line to override the default one:

```bash
dotnet run -- "Write a product launch announcement"
```

### Environment variables

| Provider | Default environment variable |
|----------|------------------------------|
| GitHub   | `GITHUB_TOKEN`               |
| OpenAI   | `OPENAI_API_KEY`             |

You can override the default by setting `apiKeyEnvironmentVariable` or `apiKey` on each agent definition.
