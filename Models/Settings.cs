using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jane2.Models
{
    public class Settings
    {
        public required string Token { get; set; }

        public required ulong AnnounceChannel { get; set; }

        public required ulong BotChannel { get; set; }

        public required string HouseholdConnection { get; set; }
    }
}