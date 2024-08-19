﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class TelegramAPIOptions
    {
        public const string OptionsName = "TelegramOptions";
        public required string TgBotToken { get; set; }
        public long HostTgId { get; set; }
        public int HostAuthAccesMinutes { get; set; }
    }
}
