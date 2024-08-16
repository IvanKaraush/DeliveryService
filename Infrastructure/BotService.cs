using Domain.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BotApi.Start(cancellationToken);
            CongratulationTask(cancellationToken);
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
                BotApi.SendCongratulationsToUsers(ids, cancellationToken).Start();
            }
        }
    }
}
