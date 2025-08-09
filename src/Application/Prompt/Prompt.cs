using Common.ConfigService;
using Core.Prompt;
using Infrastructure.Prompt.Gpt;
using Ports.Prompt;

namespace Application.Prompt;

public class PromptApp : IPromptSimplePort, IPromptStreamingPort
{
    private readonly PromptCore _promptService;

    public PromptApp()
    {
        var gptEndpoint = Config.GetOrThrow<string>("GPT_ENDPOINT");
        var gptKey = Config.GetOrThrow<string>("GPT_KEY");
        var gptModel = Config.GetOrThrow<string>("GPT_MODEL");

        var options = new PromptInstructionOptions(
            singleQueryOnly: true,
            length: PromptLength.Short,
            style: PromptStyle.Playful
        );

        var instruction = new PromptInstruction(options);
        var gptAdapter = new GptAdapter(gptEndpoint, gptKey, gptModel, instruction);

        this._promptService = new PromptCore(
            streamingGenerator: gptAdapter,
            simpleGenerator: gptAdapter
        );
    }

    Task<PromptOut> IPromptSimplePort.GeneratePrompt(string prompt)
    {
        return this._promptService.GeneratePrompt<Task<PromptOut>>(prompt);
    }

    IAsyncEnumerable<PromptOut> IPromptStreamingPort.GeneratePrompt(string prompt)
    {
        return this._promptService.GeneratePrompt<IAsyncEnumerable<PromptOut>>(prompt);
    }

    public T SendMessage<T>(string prompt) where T : class
    {
        return this._promptService.GeneratePrompt<T>(prompt);
    }
}
