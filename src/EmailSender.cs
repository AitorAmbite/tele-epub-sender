using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TeleEpubSender;

public class EmailSender(IOptions<EmailSettings> options)
{
    private readonly EmailSettings _s = options.Value;

    public async Task SendAsync(string fileName, Stream content, CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_s.From));
        message.To.Add(MailboxAddress.Parse(_s.To));
        message.Subject = fileName;

        var body = new BodyBuilder { TextBody = fileName };
        await body.Attachments.AddAsync(fileName, content, ContentType.Parse("application/epub+zip"), ct);
        message.Body = body.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_s.SmtpHost, _s.SmtpPort, SecureSocketOptions.StartTls, ct);
        await smtp.AuthenticateAsync(_s.User, _s.Password, ct);
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }
}
