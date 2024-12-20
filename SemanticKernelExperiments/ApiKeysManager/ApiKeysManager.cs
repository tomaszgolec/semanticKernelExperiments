
using Microsoft.Extensions.Configuration;

public class ApiKeysManager
{
    private static readonly Lazy<ApiKeysManager> _instance =
        new Lazy<ApiKeysManager>(() => new ApiKeysManager());
    private readonly IConfigurationRoot _config;


    private ApiKeysManager()
    {

        _config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
    }

    public static ApiKeysManager Instance => _instance.Value;

    public string GetApiKey(string keyName)
    {
        var key = _config[$"Keys:{keyName}"];
        return key ?? throw new KeyNotFoundException($"API Key: {keyName} is not found.");
    }
}

