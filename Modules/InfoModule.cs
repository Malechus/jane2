using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jane2.Models;
using jane2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace jane2.Modules
{
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		private static IServiceProvider serviceProvider;
		private static CommandService commandService;
		private static CommandHelper commandHelper;

		public InfoModule(IServiceProvider services)
		{
			serviceProvider = services;
			commandService = services.GetRequiredService<CommandService>();
			commandHelper = new CommandHelper(commandService, serviceProvider);
		}

		[Group("List")]
		public class ListModule : ModuleBase<SocketCommandContext>
		{
			[Command("all")]
			[Summary("Echos a list of all available commands.")]
			[Alias("commands")]
			public async Task ListAllAsync()
			{
				await ReplyAsync("Okay! Here are all the commands I can offer:");

				string _result = commandHelper.GetCommandsList();

				await ReplyAsync(_result);
			}
		}

		[Command("Jane")]
		[Summary("Triggers a response from JANE.")]
		[Alias("Say")]
		public async Task JaneSay()
		{
			string _response = commandHelper.GetRandomResponse();

			await ReplyAsync(_response);
		}
	}
}