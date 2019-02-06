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
        private TConfigFile botCgf;
        private DiscordChannel loggingChannel;
        private bool enableLogging = false;
        public DiscordBot()
        {
            Logger.discordBot = this;
            RunBotAsync().GetAwaiter().GetResult();
        }
        
        private async Task RunBotAsync() {
            MakeTConfig.CreateDiscord();
            botCgf = new TConfigFile("Bot_Settings.tcfg");

            var cfg = new DiscordConfiguration
            {
                Token = botCgf.Read("Bot Token"),
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
            game.Name = botCgf.Read("Status");
            game.StartTimestamp = DateTimeOffset.Now;
            Client.UpdateStatusAsync(game);
            loggingChannel = Client.GetChannelAsync(ulong.Parse(botCgf.Read("Channel"))).Result;
            enableLogging = true;
            return Task.CompletedTask;
        }

        public void LogMessage(string message)
        {
            if(enableLogging) loggingChannel.SendMessageAsync("```" + message + "```");
        }
    }
}
