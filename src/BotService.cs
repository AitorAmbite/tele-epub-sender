using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TeleEpubSender;

public class BotService(
    IOptions<TelegramSettings> options,
    EmailSender email,
    ILogger<BotService> log) : BackgroundService
{
    private readonly TelegramSettings _s = options.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var bot = new TelegramBotClient(_s.BotToken, cancellationToken: ct);
        bot.OnMessage += (msg, _) => HandleAsync(bot, msg, ct);
        bot.OnError += (ex, _) => { log.LogError(ex, "Error de Telegram"); return Task.CompletedTask; };

        log.LogInformation("Bot @{User} escuchando", (await bot.GetMe(ct)).Username);
        await Task.Delay(Timeout.Infinite, ct);
    }

    private async Task HandleAsync(TelegramBotClient bot, Message msg, CancellationToken ct)
    {
        if (msg.Text?.Split('@')[0] == "/id")
        {
            await bot.SendMessage(msg.Chat, $"Tu id: {msg.From?.Id}\nChat id: {msg.Chat.Id}", cancellationToken: ct);
            return;
        }

        var doc = msg.Document;
        if (doc is null) return;
        if (_s.AllowedChatIds.Length > 0 && !_s.AllowedChatIds.Contains(msg.Chat.Id)) return;

        var name = doc.FileName ?? "";
        if (!name.EndsWith(".epub", StringComparison.OrdinalIgnoreCase))
        {
            await bot.SendMessage(msg.Chat, "Solo acepto archivos .epub", cancellationToken: ct);
            return;
        }

        try
        {
            await using var stream = new MemoryStream();
            await bot.GetInfoAndDownloadFile(doc.FileId, stream, ct);
            stream.Position = 0;
            await email.SendAsync(name, stream, ct);
            await bot.SendMessage(msg.Chat, $"✅ {name} enviado por correo", cancellationToken: ct);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Fallo enviando {File}", name);
            await bot.SendMessage(msg.Chat, $"❌ No se pudo enviar: {ex.Message}", cancellationToken: ct);
        }
    }
}
