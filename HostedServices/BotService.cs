using Domain.Stores;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BotService : IHostedService
    {
        public BotService(ITelegramBotApi botApi, IServiceScopeFactory scopeFactory)
        {
            BotApi = botApi;
            UserStore = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserStore>();
        }
        ITelegramBotApi BotApi;
        IUserStore UserStore;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            BotApi.Start(cancellationToken);
            var st = Environment.StackTrace;
            Exception ex = new Exception();
            #pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            CongratulationTask(cancellationToken);
            #pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await BotApi.Stop();
        }
        private async Task CongratulationTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
                List<string> ids = await UserStore.GetBirthdayPeopleTelegram();
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
                BotApi.SendCongratulationsToUsers(ids, cancellationToken);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            }
        }
    }
}
