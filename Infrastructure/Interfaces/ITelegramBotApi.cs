using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITelegramBotApi
    {
        public Task SendCongratulationsToUsers(List<string> idList, CancellationToken cancellationToken);
        public Task SendHostAuthMessage();
        public void Start(CancellationToken cancellationToken);
        public Task Stop();
        public bool IsHostLogined { get; }
    }
}
