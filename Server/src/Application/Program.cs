using Common.ConfigService;
using Core.Prompt;
using Core.Prompt.Types;
using Infrastructure.Prompt.Gpt;
using Ports.Prompt;

namespace Application;

class Program
{
    static async Task Main(string[] args)
    {
        var gptEndpoint = Config.GetOrThrow<string>("GPT_ENDPOINT");
        var gptKey = Config.GetOrThrow<string>("GPT_KEY");
        var gptModel = Config.GetOrThrow<string>("GPT_MODEL");

        var options = new PromptInstructionOptions(
            singleQueryOnly: true,
            length: PromptLength.Medium,
            style: PromptStyle.Playful
        );

        var instruction = new PromptInstruction(options);
        var gptAdapter = new GptAdapter(gptEndpoint, gptKey, gptModel, instruction);
        var promptService = new Prompt(streamingGenerator: gptAdapter, simpleGenerator: gptAdapter);

        Console.Write("\nSimple Response: ");
        var promptSimple = await promptService.GeneratePrompt<Task<PromptOut>>("I've wanted to confess for a long time that I love you.");
        Console.Write(promptSimple.response);

        Console.Write("\nStream Response: ");
        var promptStream = promptService.GeneratePrompt<IAsyncEnumerable<PromptOut>>("Would you mind being my girlfriend?");
        await foreach (var chunk in promptStream)
        {
            Console.Write(chunk.response);
        }

    }
}
