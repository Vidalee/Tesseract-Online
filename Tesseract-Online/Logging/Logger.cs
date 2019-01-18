using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    static class Logger
    {
        private static readonly ConsoleColor INFO_COLOR = ConsoleColor.Cyan;
        private static readonly ConsoleColor ERROR_COLOR = ConsoleColor.Red;
        private static readonly ConsoleColor WARNING_COLOR = ConsoleColor.DarkYellow;

        internal static DiscordBot discordBot;

        public static void Init()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                new DiscordBot();
            }).Start();
        }
        
        public static void INFO(string message)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = INFO_COLOR;
            message = GetTimestamp(DateTime.Now) + "-[INFO]: " + message;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
            discordBot.LogMessage(message);
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("[yyyy/MM/dd-HH:mm:ss]");
        }
    }
}
