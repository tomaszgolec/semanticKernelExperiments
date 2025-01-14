using AIParserTestApp;
using AIParserTestApp.Models;


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

var aiSerializer = new AiSerializerPrototype(llmModel, apiKey);

ExampleModel model = await aiSerializer.Deserialize<ExampleModel>(exampleJson);


Console.WriteLine(model.ToString());

