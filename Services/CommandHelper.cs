using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Discord.Commands;
using Discord.WebSocket;
using jane2.Models;
using jane2.Modules;
using jane2.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace jane2.Services
{
	public class CommandHelper
	{
		private readonly CommandService commandService;
		private readonly IServiceProvider serviceProvider;

		public CommandHelper(CommandService CommandService, IServiceProvider ServiceProvider)
		{
			commandService = CommandService;

			serviceProvider = ServiceProvider;
		}

		public string GetCommandsList()
		{
			List<CommandInfo> _commandList = commandService.Commands.ToList();

			StringBuilder _builder = new StringBuilder();

			_builder.AppendLine("The following commands are available:");

			List<CommandInfo> _listCommands = new List<CommandInfo>();
			List<CommandInfo> _standAloneCommands = new List<CommandInfo>();

			foreach (CommandInfo c in _commandList)
			{
				switch (c.Module.Group)
				{
					case "List":
						_listCommands.Add(c);
						break;
					case null:
						_standAloneCommands.Add(c);
						break;
					default:
						_standAloneCommands.Add(c);
						break;
				}
			}

			_builder.AppendLine("List Commands:");
			foreach (CommandInfo c in _listCommands)
			{
				_builder.AppendLine("List " + c.Name + "....." + c.Summary);
			}

			_builder.AppendLine("Stand Alone Commands:");
			foreach (CommandInfo c in _standAloneCommands)
			{
				_builder.AppendLine(c.Name + "....." + c.Summary);
			}

			return _builder.ToString();
		}

		public string GetRandomResponse()
		{
			List<string> _responses = serviceProvider.GetRequiredService<IConfigurationRoot>().GetRequiredSection("Responses").Get<List<string>>();

			Random _rand = serviceProvider.GetRequiredService<Random>();
			int _max = _responses.Count - 1;
			string _result = _responses[_rand.Next(0, _max)];

			return _result;
		}
	}
}