using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIParserTestApp
{
    public class AiSerializerPrototype
    {
        private readonly string _model;
        private readonly string _apiKey;


        public AiSerializerPrototype(string model, string apiKey)
        {
            _model = model;
            _apiKey = apiKey;
        }


        //todo make it more inteligent or create another function with higher inteligence where
        //todo chat will be able to decide if delivered data are enough to fullfill the model and create the instance
        public async Task<T> Deserialize<T>(string json)
        {

            IKernelBuilder builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(_model, _apiKey);

            builder.Plugins.AddFromType<ModelCreatorPlugin<T>>();

            Kernel kernel = builder.Build();

            IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            
            PromptExecutionSettings settings = new() { FunctionChoiceBehavior = Microsoft.SemanticKernel.FunctionChoiceBehavior.Required(autoInvoke: false) };

            ChatHistory chatHistory = [];
            chatHistory.AddUserMessage(json);

            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);

            if (result.Content is not null)
            {
                //throw expetion here that somehow model gerenate response before usage of a tool
            }

            chatHistory.Add(result);

            IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);
            if (!functionCalls.Any())
            {
                //throw Exception that that plugin is missing 
            }

            var functionCall = functionCalls.FirstOrDefault();
            
            FunctionResultContent resultContent = await functionCall.InvokeAsync(kernel);
            T model = (T)resultContent.Result;

            return model;
        }

    }
}
