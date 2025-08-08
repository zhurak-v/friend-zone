using Ports.Prompt;

namespace Core.Prompt;

public class Prompt : IPromptSimplePort, IPromptStreamingPort
{
    private IPromptSimplePort? _simpleGenerator;
    private IPromptStreamingPort? _streamingGenerator;

    public Prompt(IPromptSimplePort? simpleGenerator = null, IPromptStreamingPort? streamingGenerator = null)
    {
        this._simpleGenerator = simpleGenerator;
        this._streamingGenerator = streamingGenerator;

        if (this._simpleGenerator == null && this._streamingGenerator == null)
        {
            throw new InvalidOperationException("No generator has been set.");
        }
    }

    public void ReloadSimple(IPromptSimplePort newSimpleGenerator)
    {
        this._simpleGenerator = newSimpleGenerator;
    }
    public void ReloadStreaming(IPromptStreamingPort newStreamingGenerator)
    {
        this._streamingGenerator = newStreamingGenerator;
    }

    Task<PromptOut> IPromptSimplePort.GeneratePrompt(string prompt)
    {
        if (this._simpleGenerator == null)
        {
            throw new InvalidOperationException("Simple generator not set.");
        }
        return this._simpleGenerator.GeneratePrompt(prompt);
    }

    IAsyncEnumerable<PromptOut> IPromptStreamingPort.GeneratePrompt(string prompt)
    {
        if (this._streamingGenerator == null)
        {
            throw new InvalidOperationException("Streaming generator not set.");
        }
        return this._streamingGenerator.GeneratePrompt(prompt);
    }

    public T GeneratePrompt<T>(string prompt) where T : class
    {
        if (typeof(T) == typeof(Task<PromptOut>))
        {
            return (T)(object)((IPromptSimplePort)this).GeneratePrompt(prompt);
        }

        if (typeof(T) == typeof(IAsyncEnumerable<PromptOut>))
        {
            return (T)(object)((IPromptStreamingPort)this).GeneratePrompt(prompt);
        }

        throw new InvalidOperationException($"Unsupported return type {typeof(T).Name}");
    }
}

