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