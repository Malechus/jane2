using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace jane2.Services
{
	public class CommandHandler
	{
		private readonly DiscordSocketClient socketClient;
		private readonly CommandService commandService;
		private readonly IServiceProvider serviceProvider;
		private readonly IConfigurationRoot configRoot;

		public CommandHandler(
			DiscordSocketClient SocketClient,
			CommandService CommandService,
			IServiceProvider ServiceProvider,
			IConfigurationRoot ConfigRoot
		)
		{
			socketClient = SocketClient;
			commandService = CommandService;
			serviceProvider = ServiceProvider;
			configRoot = ConfigRoot;

			socketClient.MessageReceived += OnMessageReceivedAsync;
		}

		private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
		{
			SocketUserMessage _message = socketMessage as SocketUserMessage;
			if (_message == null) return;
			if (_message.Author.Id == socketClient.CurrentUser.Id) return;

			SocketCommandContext _context = new SocketCommandContext(socketClient, _message);

			int _argPos = 0;

			if (_message.HasCharPrefix('!', ref _argPos) || _message.HasMentionPrefix(socketClient.CurrentUser, ref _argPos))
			{
				IResult? _result = await commandService.ExecuteAsync(_context, _argPos, serviceProvider);

				if (!_result.IsSuccess)
				{
					await _context.Channel.SendMessageAsync("I'm so sorry, I couldn't find that command. Want to try that again?");
				}
			}
		}
	}
}