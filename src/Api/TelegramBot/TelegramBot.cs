using Ports.Prompt;
using Telegram.Bot;

namespace Api.TelegramBot;

public class TelegramBotApi
{
    private readonly TelegramBotClient _bot;
    private readonly CancellationTokenSource _cts;
    private readonly TelegramMessageHandler _messageHandler;

    public TelegramBotApi(string token, IPromptSimplePort promptPort)
    {
        this._cts = new CancellationTokenSource();
        this._bot = new TelegramBotClient(token, cancellationToken: this._cts.Token);
        this._messageHandler = new TelegramMessageHandler(this._bot, promptPort, this._cts.Token);
    }

    public async Task StartAsync()
    {
        await this._bot.DeleteWebhook();
        await this._bot.DropPendingUpdates();

        this._bot.OnMessage += async (sender, e) =>
        {
            await this._messageHandler.HandleMessage(sender);
        };

        try
        {
            await Task.Delay(Timeout.Infinite, this._cts.Token);
        }
        catch (TaskCanceledException) { }
    }
}