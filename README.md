<img src="assets/icon.svg" width="64" height="64" alt="">

# tele-epub-sender

Bot de Telegram: le envías un `.epub` y lo reenvía por correo a la dirección configurada.

## Configuración (`src/appsettings.json`)

| Clave | Descripción |
|---|---|
| `Telegram:BotToken` | Token de @BotFather |
| `Telegram:AllowedUserIds` | Ids de usuario permitidos, separados por comas (vacío = nadie). Cada usuario obtiene el suyo con `/id` |
| `Email:To` | Destino (p. ej. tu Kindle) |
| `Email:From` / `User` / `Password` | Remitente y credenciales SMTP |
| `Email:SmtpHost` / `SmtpPort` | Servidor SMTP (STARTTLS) |

Mejor no guardar secretos en el fichero: `dotnet user-secrets` o variables de entorno (`Telegram__BotToken=...`).

## Ejecutar

```
dotnet run --project src
```

## Docker

```
cp .env.example .env   # rellénalo
docker compose up -d --build
docker compose logs -f
```
