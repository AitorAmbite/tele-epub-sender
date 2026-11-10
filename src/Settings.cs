namespace TeleEpubSender;

public record TelegramSettings
{
    public string BotToken { get; init; } = "";
    /// <summary>Ids de usuario permitidos, separados por comas. Vacío = nadie.</summary>
    public string AllowedUserIds { get; init; } = "";

    public bool IsAllowed(long? userId) =>
        userId is { } id && AllowedUserIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(a => a == id.ToString());
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
