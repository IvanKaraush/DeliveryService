using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ITelegramBotApi
    {
        Task SendCongratulationsToUsers(List<int> idList);
        Task SendHostAuthMessage();
        void Start(CancellationToken cancellationToken);
        bool IsHostLogined { get; }
    }
}
