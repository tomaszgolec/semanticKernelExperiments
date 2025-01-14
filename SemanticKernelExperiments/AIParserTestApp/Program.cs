using AIParserTestApp;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


var apiKey = ApiKeysManager.Instance.GetApiKey("OpenApiKey");
var llmModel = "gpt-3.5-turbo";
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

var aiParser = new AiParserPrototype(llmModel, apiKey);
var model2 = await aiParser.NoGenericParse(exampleJson);

Console.WriteLine(model2.ToString());

