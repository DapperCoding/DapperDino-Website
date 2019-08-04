using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DapperDino.Core.Discord
{
    // SingleTon
    public class Bot : IDisposable
    {
        private DiscordClient _client;
        private StartTimes _starttimes;
        private CancellationTokenSource _cts;

        public Bot(IConfiguration configuration)
        {

            _client = new DiscordClient(new DiscordConfiguration()
            {
                AutoReconnect = true,
                EnableCompression = true,
                LogLevel = LogLevel.Debug,
                Token = configuration["DiscordToken"],
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true
            });

            _starttimes = new StartTimes()
            {
                BotStart = DateTime.Now,
                SocketStart = DateTime.MinValue
            };

            _cts = new CancellationTokenSource();


            _client.Ready += OnReadyAsync;
        }

        public DiscordClient GetClient()
        {
            return _client;
        }

        public async Task RunAsync()
        {
            await _client.ConnectAsync();
            await WaitForCancellationAsync();
        }

        private async Task WaitForCancellationAsync()
        {
            while (!_cts.IsCancellationRequested)
                await Task.Delay(500);
        }

        private async Task OnReadyAsync(ReadyEventArgs e)
        {
            await Task.Yield();
            _starttimes.SocketStart = DateTime.Now;
        }

        public void Dispose()
        {
            try
            {
                this._client?.Dispose();
            }
            catch (Exception e)
            {

            }

        }

        internal void WriteCenter(string value, int skipline = 0)
        {
            for (int i = 0; i < skipline; i++)
                Console.WriteLine();

            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
        }


    }
}
