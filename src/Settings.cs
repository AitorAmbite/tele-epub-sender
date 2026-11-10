namespace TeleEpubSender;

public record TelegramSettings
{
    public string BotToken { get; init; } = "";
    /// <summary>Si está vacío, cualquier chat puede usar el bot.</summary>
    public long[] AllowedChatIds { get; init; } = [];
}

public record EmailSettings
{
    public string To { get; init; } = "";
    public string From { get; init; } = "";
    public string SmtpHost { get; init; } = "";
    public int SmtpPort { get; init; } = 587;
    public string User { get; init; } = "";
    public string Password { get; init; } = "";
}
