using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Domain.Models.ApplicationModels;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class TelegramBotAPI : ITelegramBotApi
    {
        public TelegramBotAPI(IOptions<TelegramAPIOptions> options, ILogger<TelegramBotAPI> logger)
        {
            Client = new TelegramBotClient(options.Value.TgBotToken);
            HostId = options.Value.HostTgId;
            Logger = logger;
            HostAuthAccesMinutes = options.Value.HostAuthAccesMinutes;
        }
        private readonly long HostId;
        private readonly int HostAuthAccesMinutes;
        private readonly ILogger<TelegramBotAPI> Logger;
        private readonly ITelegramBotClient Client;
        private object Locker = new();
        private bool HostLoginedFlag = false;
        public bool IsHostLogined
        {
            get
            {
                lock (Locker)
                {
                    return HostLoginedFlag;
                }
            }
            private set
            {
                lock (Locker)
                {
                    HostLoginedFlag = value;
                }
            }
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update?.Type == UpdateType.Message)
            {
                var message = update.Message;                
                if (message.Chat.Type == ChatType.Private && message.Chat.Id == HostId)
                    if (message?.Text != null)
                    {
                        string messageText = message.Text.ToLower();
                        if (messageText.Contains("login") || messageText.Contains("войти") || messageText.Contains("вход"))
                        {
                            if (!IsHostLogined)
                            {
                                OpenHostAuthAccesValue(cancellationToken);
                                await Client.SendTextMessageAsync(message.Chat, "OK");
                            }
                            else
                                await Client.SendTextMessageAsync(message.Chat, "Acces to authorization is already opened | Доступ к авторизации уже открыт");
                        }
                        else await Client.SendTextMessageAsync(message.Chat, "Unknown command | Неизвестная команда");
                    }
            }
        }
        private async Task OpenHostAuthAccesValue(CancellationToken cancellationToken)
        {
            IsHostLogined = true;
            await Task.Delay(TimeSpan.FromMinutes(HostAuthAccesMinutes), cancellationToken);
            IsHostLogined = false;
        }
        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => Logger?.LogError(exception.Message));
        }
        public async void Start(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
            await Client.SendTextMessageAsync(new ChatId(HostId), "Application launched | Приложение запущено");
        }
        public async Task Stop()
        {
            await Client.SendTextMessageAsync(new ChatId(HostId), "Application stopped | Приложение остановлено");
        }
        public async Task SendCongratulationsToUsers(List<string> idList, CancellationToken cancellationToken)
        {
            foreach (var id in idList)
            {
                if (!cancellationToken.IsCancellationRequested)
                    try
                    {
                        await Client.SendTextMessageAsync(new ChatId(id), "Поздравляем с днем рождения", cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Can\'t send congratulations to ID: {id}\nMessage: {ex.Message}");
                    }
            }
        }
        public async Task SendHostAuthMessage()
        {
            await Client.SendTextMessageAsync(new ChatId(HostId), "Login attempt, enter \"login\" | Попытка входа, введите \"вход\"");
        }
    }
}
