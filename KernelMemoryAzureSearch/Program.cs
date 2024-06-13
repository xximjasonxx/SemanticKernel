
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0020
#pragma warning disable SKEXP0010

using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Config>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
    deploymentName: config["DeploymentName"],
    endpoint: config["Endpoint"],
    apiKey: config["Key"]);

var kernel = builder.Build();
var azureSearchExtensionConfiguration = new AzureSearchChatExtensionConfiguration
{
    SearchEndpoint = new Uri(config["SearchEndpoint"]),
    Authentication = new OnYourDataApiKeyAuthenticationOptions(config["SearchApiKey"]),
    IndexName = config["SearchIndexName"],
};

var chatExtensionsOptions = new AzureChatExtensionsOptions { Extensions = { azureSearchExtensionConfiguration } };
var executionSettings = new OpenAIPromptExecutionSettings { AzureChatExtensionsOptions = chatExtensionsOptions };

string question = args[0];
Console.WriteLine($"Asking: {question}");

var response = await kernel.InvokePromptAsync(question, new(executionSettings));
Console.WriteLine(response);

public class Config
{
    public string DeploymentName { get; set; }
    public string Endpoint { get; set; }
    public string Key { get; set; }
    }