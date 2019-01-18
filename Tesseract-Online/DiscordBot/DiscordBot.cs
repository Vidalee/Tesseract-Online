using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class DiscordBot
    {
        private readonly string pf = "!";
        private DiscordClient Client { get; set; }
        private IniFile botIni;
        private DiscordChannel loggingChannel;
        private bool enableLogging = false;
        public DiscordBot()
        {
            Logger.discordBot = this;
            RunBotAsync().GetAwaiter().GetResult();
        }
        
        private async Task RunBotAsync() {
            MakeIni.Create();
            botIni = new IniFile("Bot_Settings.ini");

            var cfg = new DiscordConfiguration
            {
                Token = botIni.Read("Bot Token"),
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };
            Client = new DiscordClient(cfg);
            Client.Ready += Client_Ready;
            // finally, let's connect and log in
            await Client.ConnectAsync();

            // and this is to prevent premature quitting
            await Task.Delay(-1);
        }

        private Task Client_Ready(ReadyEventArgs e)
        {
            Console.WriteLine("Connected!");
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", "Client is ready to process events.", DateTime.Now);
            DiscordGame game = new DiscordGame();
            game.Name = botIni.Read("Status");
            game.StartTimestamp = DateTimeOffset.Now;
            Client.UpdateStatusAsync(game);
            loggingChannel = Client.GetChannelAsync(ulong.Parse(botIni.Read("Channel"))).Result;
            enableLogging = true;
            return Task.CompletedTask;
        }

        public void LogMessage(string message)
        {
            if(enableLogging) loggingChannel.SendMessageAsync("```" + message + "```");
        }
    }
}
