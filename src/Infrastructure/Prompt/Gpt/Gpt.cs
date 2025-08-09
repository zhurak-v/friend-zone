using Azure;
using Azure.AI.OpenAI;
using Core.Prompt;
using OpenAI.Chat;
using Ports.Prompt;

namespace Infrastructure.Prompt.Gpt;

public class GptAdapter : IPromptSimplePort, IPromptStreamingPort
{
    private readonly ChatClient _chatClient;
    private readonly ChatCompletionOptions _chatOptions;
    private readonly PromptInstruction _instructionProvider;

    public GptAdapter(string gptEndpoint, string gptKey, string gptModel, PromptInstruction instructionProvider)
    {
        var uri = new Uri(gptEndpoint);
        var credential = new AzureKeyCredential(gptKey);
        var client = new AzureOpenAIClient(uri, credential);

        this._chatClient = client.GetChatClient(gptModel);
        this._instructionProvider = instructionProvider;

        this._chatOptions = new ChatCompletionOptions
        {
            Temperature = 0.3f,
            MaxOutputTokenCount = 300,
            TopP = 0.8f,
            PresencePenalty = 0.7f,
        };
    }

    async Task<PromptOut> IPromptSimplePort.GeneratePrompt(string prompt)
    {
        var instruction = this._instructionProvider.GetInstructions();

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(instruction),
            new UserChatMessage(prompt)
        };

        try
        {
            var result = await _chatClient.CompleteChatAsync(messages, _chatOptions);
            var responseText = result.Value.Content[0].Text;
            return new PromptOut(responseText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in simple GeneratePrompt: {ex.Message}");
            return new PromptOut("[Error]");
        }
    }

    async IAsyncEnumerable<PromptOut> IPromptStreamingPort.GeneratePrompt(string prompt)
    {
        var instruction = this._instructionProvider.GetInstructions();

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(instruction),
            new UserChatMessage(prompt)
        };

        var completionUpdates = this._chatClient.CompleteChatStreamingAsync(messages, _chatOptions);

        await foreach (var update in completionUpdates)
        {
            string partial = "";

            foreach (var part in update.ContentUpdate)
            {
                partial += part.Text;
            }
            if (!string.IsNullOrEmpty(partial))
            {
                yield return new PromptOut(partial);
            }
        }
    }
}