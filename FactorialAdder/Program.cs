
using FactorialAdder;
using FactorialAdder.Plugins;
using HandlebarsDotNet.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

int numberOne = int.Parse(args[0]);
int numberTwo = int.Parse(args[1]);

// configuration
var config = new ConfigurationBuilder()
    .AddUserSecrets<Config>()
    .Build();

// build our math kernel
var builder = Kernel.CreateBuilder();
builder.Plugins.AddFromType<MathPlugin>();
builder.Plugins.AddFromPromptDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "GenStatementPlugin"));
builder.AddAzureOpenAIChatCompletion(config["DeploymentName"], config["Endpoint"], config["Key"]);

var kernel = builder.Build();

#pragma warning disable // Suppress the diagnostic messages

var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions() { AllowLoops = true });
var plan = await planner.CreatePlanAsync(kernel, $"Generate a statement that uses the result of subtracting 10 from the sum of factorials {numberOne} and {numberTwo}");
var planResult = await plan.InvokeAsync(kernel, new KernelArguments());

Console.WriteLine(planResult);