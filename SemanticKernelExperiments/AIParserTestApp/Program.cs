using AIParserTestApp;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


var apiKey = ApiKeysManager.Instance.GetApiKey("OpenApiKey");
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

IKernelBuilder builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion("gpt-3.5-turbo", apiKey);
builder.Plugins.AddFromType<ExampleModel>();

Kernel kernel = builder.Build();

IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Manual function invocation needs to be enabled explicitly by setting autoInvoke to false.
PromptExecutionSettings settings = new() { FunctionChoiceBehavior = Microsoft.SemanticKernel.FunctionChoiceBehavior.Required(autoInvoke: false) };

ChatHistory chatHistory = [];
chatHistory.AddUserMessage(exampleJson);

ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);

// Check if the AI model has generated a response.
if (result.Content is not null)
{
    //throw expetion here that somehow model gerenate response before usage of a tool
}

chatHistory.Add(result);

// Check if the AI model has chosen any function for invocation.
IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);
if (!functionCalls.Any())
{
    //throw Exception that that plugin is missing 
}

var functionCall = functionCalls.FirstOrDefault();
// Invoking the function
FunctionResultContent resultContent = await functionCall.InvokeAsync(kernel);
ExampleModel model = (ExampleModel)resultContent.Result;

Console.ReadKey();