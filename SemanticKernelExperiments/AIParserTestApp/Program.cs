using AIParserTestApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

string exampleJson = @"
{
    ""userId"": ""12345"",
    ""name"": ""John Doe"",
    ""preferences"": {
        ""lang"": ""English"",
        ""theme"": ""dark""
    },
    ""isActive"": true
}";

//TODO jesli nei da sie odczytac tego tak po prostu z wyniku 
//to moze sprobuj wrzucic mu klase po refleksji zbey po prostu dopasowal jsona tak 
//zeby pasowal do obiektu

var modelId = "gpt-3.5-turbo";
var apiKey = ApiKeysManager.Instance.GetApiKey("OpenApiKey");

var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId, apiKey);

builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

Kernel kernel = builder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

kernel.Plugins.AddFromType<ExampleModel>("ExampleModel");

OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var history = new ChatHistory();

string? userInput;

Console.Write("User > ");
userInput = $"Sparsuj nastepujacy json do klasy ExampleModel :{exampleJson}";

history.AddUserMessage(userInput);

var result = await chatCompletionService.GetChatMessageContentAsync(
    history,
    executionSettings: openAIPromptExecutionSettings,
    kernel: kernel);


Console.WriteLine("Assistant > " + result);

history.AddMessage(result.Role, result.Content ?? string.Empty);
