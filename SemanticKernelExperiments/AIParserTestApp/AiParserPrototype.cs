using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIParserTestApp
{
    public class AiParserPrototype
    {
        private readonly string _model;
        private readonly string _apiKey;


        public AiParserPrototype(string model, string apiKey)
        {
            _model = model;
            _apiKey = apiKey;
        }

        public async Task<T> Parse<T>(string json)
        {

            IKernelBuilder builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(_model, _apiKey);
            //add type ass pareameter to make it generic 
            builder.Plugins.AddFromType<T>();

            Kernel kernel = builder.Build();

            IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Manual function invocation needs to be enabled explicitly by setting autoInvoke to false.
            PromptExecutionSettings settings = new() { FunctionChoiceBehavior = Microsoft.SemanticKernel.FunctionChoiceBehavior.Required(autoInvoke: false) };

            ChatHistory chatHistory = [];
            chatHistory.AddUserMessage(json);

            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);

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
            T model = (T)resultContent.Result;

            return model;
        }

    }
}
