using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace discord_project
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _command;
        private IServiceProvider _service;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _command = new CommandService();
            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_command)
                .BuildServiceProvider();

            string botPrefix = "";

            //event subscription
            _client.Log += Log;

            await RegisterCommandAsync();

            await _client.LoginAsync(TokenType.Bot, botPrefix);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage log)
        {
            Console.WriteLine(log);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _service);

        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
            {
                return;
            }

            int argumentPosition = 0;

            if (message.HasStringPrefix("bot!", ref argumentPosition) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _command.ExecuteAsync(context, argumentPosition, _service);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
