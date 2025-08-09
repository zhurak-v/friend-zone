using Api.TelegramBot;
using Application.Prompt;
using Common.ConfigService;
using Core.Prompt;
using Ports.Prompt;

namespace Application.TelegramBot;

public class TelegramBotApp
{
    private readonly TelegramBotApi _botService;
    private readonly PromptApp _promptService;

    public TelegramBotApp()
    {
        var telegramToken = Config.GetOrThrow<string>("TELEGRAM_KEY");

        this._promptService = new PromptApp();

        this._botService = new TelegramBotApi(telegramToken, this._promptService);
    }

    public async Task RunAsync()
    {
        await this._botService.StartAsync();
    }
}
