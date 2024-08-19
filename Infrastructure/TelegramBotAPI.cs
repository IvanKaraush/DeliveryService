﻿using Microsoft.Extensions.Options;
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
using Telegram.Bot.Exceptions;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class TelegramBotAPI : ITelegramBotApi
    {
        public TelegramBotAPI(IOptions<TelegramAPIOptions> options, ILogger<TelegramBotAPI> logger)
        {
            _client = new TelegramBotClient(options.Value.TgBotToken);
            _hostId = options.Value.HostTgId;
            _logger = logger;
            _hostAuthAccesMinutes = options.Value.HostAuthAccesMinutes;
        }
        private readonly long _hostId;
        private readonly int _hostAuthAccesMinutes;
        private readonly ILogger<TelegramBotAPI> _logger;
        private readonly ITelegramBotClient _client;
        private object _locker = new();
        private bool _hostLoginedFlag = false;
        public bool IsHostLogined
        {
            get
            {
                lock (_locker)
                {
                    return _hostLoginedFlag;
                }
            }
            private set
            {
                lock (_locker)
                {
                    _hostLoginedFlag = value;
                }
            }
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update?.Type == UpdateType.Message)
            {
                Message? message = update.Message;
                if (message == null)
                    throw new Exception("TG message is null");
                if (message.Chat.Type == ChatType.Private && message.Chat.Id == _hostId)
                    if (message?.Text != null)
                    {
                        string messageText = message.Text.ToLower();
                        if (messageText.Contains("login") || messageText.Contains("войти") || messageText.Contains("вход"))
                        {
                            if (!IsHostLogined)
                            {
                                await OpenHostAuthAccesValue(cancellationToken);
                                await _client.SendTextMessageAsync(message.Chat, "OK");
                            }
                            else
                                await _client.SendTextMessageAsync(message.Chat, "Acces to authorization is already opened | Доступ к авторизации уже открыт");
                        }
                        else await _client.SendTextMessageAsync(message.Chat, "Unknown command | Неизвестная команда");
                    }
            }
        }
        private async Task OpenHostAuthAccesValue(CancellationToken cancellationToken)
        {
            IsHostLogined = true;
            await Task.Delay(TimeSpan.FromMinutes(_hostAuthAccesMinutes), cancellationToken);
            IsHostLogined = false;
        }
        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => _logger?.LogError(exception.Message));
        }
        public async void Start(CancellationToken cancellationToken)
        {
            try
            {
                ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
                _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
                await _client.SendTextMessageAsync(new ChatId(_hostId), "Application launched | Приложение запущено");
            }
            catch (ApiRequestException)
            {
                _logger.LogCritical("Invalid telegram bot token");
                throw;
            }
        }
        public async Task Stop()
        {
            await _client.SendTextMessageAsync(new ChatId(_hostId), "Application stopped | Приложение остановлено");
        }
        public async Task SendCongratulationsToUsers(List<string> idList, CancellationToken cancellationToken)
        {
            foreach (string id in idList)
            {
                if (!cancellationToken.IsCancellationRequested)
                    try
                    {
                        await _client.SendTextMessageAsync(new ChatId(id), "Поздравляем с днем рождения", cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Can\'t send congratulations to ID: {id}\nMessage: {ex.Message}");
                    }
            }
        }
        public async Task SendHostAuthMessage()
        {
            await _client.SendTextMessageAsync(new ChatId(_hostId), "Login attempt, enter \"login\" | Попытка входа, введите \"вход\"");
        }
    }
}
