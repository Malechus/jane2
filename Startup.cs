using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jane2.Models;
using jane2.Services;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace jane2
{
	public class Startup
	{
		private IConfigurationRoot config;
		private string runtimeEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		public Startup()
		{
			if (runtimeEnvironment is null)
			{
				runtimeEnvironment = "Development";
			}

			IConfigurationBuilder _builder = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.AddJsonFile($"appsettings.{runtimeEnvironment}.json")
			.AddJsonFile("Responses.json");

			config = _builder.Build();
		}

		public static async Task StartAsync()
		{
			Startup _startup = new Startup();
			await _startup.RunAsync();
		}

		private async Task RunAsync()
		{
			ServiceCollection _services = new ServiceCollection();
			ConfigureServices(_services);

			ServiceProvider _provider = _services.BuildServiceProvider();
			_provider.GetRequiredService<LoggerService>();
			_provider.GetRequiredService<CommandHandler>();

			await _provider.GetRequiredService<StartupService>().StartConnectionAsync();
			await Task.Delay(-1);
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(config)
			.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Verbose,
				MessageCacheSize = 1000,
				GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
			}))
			.AddSingleton(new CommandService(new CommandServiceConfig
			{
				LogLevel = LogSeverity.Verbose,
				DefaultRunMode = RunMode.Async
			}))
			.AddSingleton<CommandHandler>()
			.AddSingleton<StartupService>()
			.AddSingleton<LoggerService>()
			.AddSingleton<CommandHelper>()
			.AddSingleton<Random>();
		}
	}
}