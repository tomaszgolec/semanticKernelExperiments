using AIParserTestApp;


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

ExampleModel model = await aiParser.Parse<ExampleModel>(exampleJson);

Console.WriteLine(model2.ToString());

