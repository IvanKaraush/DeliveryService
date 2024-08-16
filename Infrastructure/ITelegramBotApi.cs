using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ITelegramBotApi
    {
        Task SendCongratulationsToUsers(List<string> idList, CancellationToken cancellationToken);
        Task SendHostAuthMessage();
        void Start(CancellationToken cancellationToken);
        Task Stop();
        bool IsHostLogined { get; }
    }
}
