namespace Common.ConfigService;

public static class Config
{
    private static readonly ConfigService _instance;
    private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new Exception("ASPNETCORE_ENVIRONMENT is not set");

    static Config()
    {
        _instance = new ConfigService($"configs/{env}.env");
    }

    public static T GetOrThrow<T>(string key)
    {
        return _instance.GetOrThrow<T>(key);
    }

    public static bool IsProduction()
    {
        return env == "production";
    }
}
