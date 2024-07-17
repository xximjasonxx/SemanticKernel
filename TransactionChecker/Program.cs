
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using TransactionChecker;
using TransactionChecker.Plugins;

Console.Write("Enter a request: ");
string requestString = Console.ReadLine() ?? string.Empty;
//var requestString = "How much did I spend on Fitness in June 2024?";

var config = new ConfigurationBuilder()
    .AddUserSecrets<Config>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton<IConfiguration>(config);

builder.Plugins.AddFromType<CosmosPlugin>();
builder.Plugins.AddFromPromptDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "FormatterPlugin"));
builder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-gs-deployment",
    endpoint: "https://openai-rag-demo-eus-jx01.openai.azure.com/",
    apiKey: "16def0ad13874d08a991fe9bfb0bcb1f");

var kernel = builder.Build();

#pragma warning disable // Suppress the diagnostic messages
var planner = new HandlebarsPlanner(
    new HandlebarsPlannerOptions() { AllowLoops = true });

var plan = await planner.CreatePlanAsync(kernel, requestString);
var planResult = await plan.InvokeAsync(kernel, new KernelArguments());

Console.WriteLine(planResult);