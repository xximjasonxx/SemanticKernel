
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using TransactionChecker;
using TransactionChecker.Plugins;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Config>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton<IConfiguration>(config);
builder.Plugins.AddFromType<CosmosPlugin>();
builder.Plugins.AddFromType<DatePlugin>();
builder.Plugins.AddFromType<TransactionsPlugin>();
builder.Plugins.AddFromPromptDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "FormatterPluginV2"));
builder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-gs-deployment",
    endpoint: config["OpenAIEndpoint"],
    apiKey: config["OpenAIApiKey"]);

var kernel = builder.Build();

do
{
    Console.Write("Enter a request: ");
    var requestString = Console.ReadLine() ?? string.Empty;

    #pragma warning disable // Suppress the diagnostic messages
    var planner = new HandlebarsPlanner(
        new HandlebarsPlannerOptions()
        {
            AllowLoops = false
        });

    var plan = await planner.CreatePlanAsync(kernel, requestString);
    var planResult = await plan.InvokeAsync(kernel, new KernelArguments());

    Console.WriteLine(planResult);
    Console.WriteLine();
} while (true);