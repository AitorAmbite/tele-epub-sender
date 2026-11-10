using TeleEpubSender;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("Telegram"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddHostedService<BotService>();
await builder.Build().RunAsync();
