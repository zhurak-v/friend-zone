namespace Ports.Prompt;

public record PromptOut(string response);

public interface IPromptStreamingPort
{
    IAsyncEnumerable<PromptOut> GeneratePrompt(string prompt);
}

public interface IPromptSimplePort
{
    Task<PromptOut> GeneratePrompt(string prompt);
}

public interface IPromptInstructionOptionsPort
{
    PromptLength GetLength();
    void SetLength(PromptLength length);

    bool GetSingleQueryOnly();
    void SetSingleQueryOnly(bool singleQueryOnly);

    PromptStyle GetStyle();
    void SetStyle(PromptStyle style);
}
