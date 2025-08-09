using Application.TelegramBot;

namespace Application.Program;

class Program
{
    static async Task Main(string[] args)
    {
        var telegramClient = new TelegramBotApp();
        await telegramClient.RunAsync();
    }
}
