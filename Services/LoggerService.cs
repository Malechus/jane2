using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace jane2.Services
{
    public class LoggerService
    {
        public LoggerService(DiscordSocketClient Client, CommandService Commands)
        {
            //Client.Log += LogEvent;
            //Commands.Log += LogEvent;
        }

        private Task LogEvent(LogMessage LogMsg)
        {
            return Task.CompletedTask;
        }
    }
}