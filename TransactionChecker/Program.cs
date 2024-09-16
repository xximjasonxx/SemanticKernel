
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;
using TransactionChecker;
using TransactionChecker.Plugins;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Config>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton<IConfiguration>(config);
builder.Plugins.AddFromType<CosmosPlugin>();
//builder.Plugins.AddFromType<DatePlugin>();
builder.Plugins.AddFromType<TransactionsPlugin>();
builder.Plugins.AddFromPromptDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "FormatterPluginV2"));
builder.Plugins.AddFromPromptDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "UtilityPlugin"));
builder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-gs-deployment",
    endpoint: config["OpenAIEndpoint"],
    apiKey: config["OpenAIApiKey"]);

var kernel = builder.Build();

#pragma warning disable // Suppress the diagnostic messages
var templateSource = "";

var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions()
{
    AllowLoops = true,
    ExecutionSettings = new OpenAIPromptExecutionSettings
    {
        Temperature = 0.0,
        TopP = 0.1,
    },
    CreatePlanPromptHandler = () =>
    {
        return templateSource;
    }
});

var chatHistory = new ChatHistory();
chatHistory.AddSystemMessage("You are a helpful assistant answering questions about spending based on a given set of transactional data");

do
{
    Console.Write("Enter a request: ");
    var requestString = Console.ReadLine() ?? string.Empty;
    chatHistory.AddUserMessage(requestString);

    var plan = await planner.CreatePlanAsync(kernel, requestString);
    var planResult = await plan.InvokeAsync(kernel, new KernelArguments() { { "chat_history", chatHistory } });
    chatHistory.AddAssistantMessage(planResult);

    Console.WriteLine(planResult);
    Console.WriteLine();
} while (true);