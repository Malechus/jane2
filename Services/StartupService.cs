using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jane2.Models;
using jane2.Services;
using Microsoft.Extensions.Configuration;

namespace jane2.Services
{
    public class StartupService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DiscordSocketClient socketClient;
        private readonly CommandService commandService;
        private readonly IConfigurationRoot configuration;
        private readonly Settings settings;

        public StartupService(IServiceProvider provider, DiscordSocketClient client, CommandService commands, IConfigurationRoot config)
        {
            serviceProvider = provider;
            socketClient = client;
            commandService = commands;
            configuration = config;
            settings = config.GetRequiredSection("Settings").Get<Settings>();
        }

        public async Task StartConnectionAsync()
        {
            socketClient.Ready += Announce;

            string _discordToken = settings.Token;
            if (string.IsNullOrWhiteSpace(_discordToken))
            {
                throw new Exception("Bad token exception.");
            }

            await socketClient.LoginAsync(TokenType.Bot, _discordToken);
            await socketClient.StartAsync();

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
        }

        private async Task Announce()
        {
            ISocketMessageChannel _channel = await socketClient.GetChannelAsync(settings.AnnounceChannel) as ISocketMessageChannel;

            await _channel.SendMessageAsync("Hiya everyone! I'm so glad to be here!");
        }
    }
}