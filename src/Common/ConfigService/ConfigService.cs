using System.Globalization;
using DotNetEnv;

namespace Common.ConfigService;

public class ConfigException : Exception
{
    private int _code;
    private object? _context;

    public ConfigException(string message, int code, object? context = null) : base(message)
    {
        this._code = code;
        this._context = context;
    }

    public int GetCode()
    {
        return this._code;
    }

    public void SetCode(int value)
    {
        this._code = value;
    }

    public object? GetContext()
    {
        return this._context;
    }

    public void SetContext(object? value)
    {
        this._context = value;
    }
}

public class ConfigService
{
    public ConfigService(string envFile = ".env")
    {
        if (File.Exists(envFile))
        {
            Env.Load(envFile);
        }
    }

    public T GetOrThrow<T>(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (value is null)
        {
            throw new ConfigException(
                $"Environment variable '{key}' is not set",
                500,
                new { key });
        }

        return this.Cast<T>(value, key);
    }

    private T Cast<T>(string value, string key)
    {
        try
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }
            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(value, out var intResult))
                    return (T)(object)intResult;
                throw new ConfigException(
                    $"Cannot cast value '{value}' to int",
                    422,
                    new { key, value });
            }
            if (typeof(T) == typeof(double))
            {
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleResult))
                    return (T)(object)doubleResult;
                throw new ConfigException(
                    $"Cannot cast value '{value}' to double",
                    422,
                    new { key, value });
            }
            if (typeof(T) == typeof(bool))
            {
                var lowered = value.ToLowerInvariant();
                if (lowered is "true" or "1" or "yes" or "on")
                    return (T)(object)true;
                if (lowered is "false" or "0" or "no" or "off")
                    return (T)(object)false;

                throw new ConfigException(
                    $"Cannot cast value '{value}' to bool",
                    422,
                    new { key, value });
            }

            throw new ConfigException(
                $"Unsupported type: {typeof(T).Name}",
                500,
                new { key, type = typeof(T).FullName });
        }
        catch (ConfigException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConfigException(
                $"Error casting value '{value}' to {typeof(T).Name}: {ex.Message}",
                422,
                new { key, value });
        }
    }
}