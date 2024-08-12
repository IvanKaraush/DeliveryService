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
        public BotService(ITelegramBotApi botApi) 
        {
            BotApi = botApi;
        }
        ITelegramBotApi BotApi;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BotApi.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
