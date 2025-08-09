using Ports.Prompt;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.TelegramBot;

public class TelegramMessageHandler
{
    private readonly IPromptSimplePort _promptPort;
    private readonly TelegramBotClient _bot;
    private readonly CancellationToken _cancellationToken;

    public TelegramMessageHandler
    (
        TelegramBotClient bot,
        IPromptSimplePort promptPort,
        CancellationToken cancellationToken
    )
    {
        this._bot = bot;
        this._promptPort = promptPort;
        this._cancellationToken = cancellationToken;
    }

    public async Task HandleMessage(Message msg)
    {
        if (msg.Text is null)
            return;

        try
        {
            string text = msg.Text!;

            switch (true)
            {
                case bool _ when text.StartsWith("/start"):
                    await HandleStartCommand(msg);
                    break;

                default:
                    await HandleDefaultMessage(msg);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при генерації відповіді: {ex.Message}");
            await this._bot.SendMessage(
                chatId: msg.Chat.Id,
                text: "Вибач, сталася помилка.",
                cancellationToken: _cancellationToken
            );
        }
    }

    private async Task HandleStartCommand(Message msg)
    {
        PromptOut prompt = await this._promptPort.GeneratePrompt("Very shortly!. Tell me what you are, a friendzone bot, and what you can do.");

        await this._bot.SendMessage(
            chatId: msg.Chat.Id,
            text: prompt.response,
            cancellationToken: this._cancellationToken
        );
    }

    private async Task HandleDefaultMessage(Message msg)
    {
        PromptOut prompt = await this._promptPort.GeneratePrompt(msg.Text!);

        await this._bot.SendMessage(
            chatId: msg.Chat.Id,
            text: prompt.response,
            cancellationToken: _cancellationToken
        );
    }
}