using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    public static class MakeTConfig
    {
        public static void CreateDiscord()
        {
            //Checks to see if Bot_Settings.tcfg exists and if it doesnt it generates a default file for you to config and then ends the bot.
            if (!File.Exists("Bot_Settings.tcfg"))
            {
                string[] defaultData = { "Bot Token=token", "Bot Name=Xelia", "Status=status", "Prefix=!", "Channel=chId" };
                File.WriteAllLines("Bot_Settings.tcfg", defaultData);

                Console.WriteLine("You did not have a tesseract discord config file set up. \nA default one has been made with the name \"Bot_Settings.tcfg\" \nPlease go ahead and configure it and restart the bot.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void CreateIRC()
        {
            //Checks to see if Bot_Settings.tcfg exists and if it doesnt it generates a default file for you to config and then ends the bot.
            if (!File.Exists("IRC_Settings.tcfg"))
            {
                string[] defaultData = { "Oper username=username", "Oper password=password"};
                File.WriteAllLines("IRC_Settings.tcfg", defaultData);

                Console.WriteLine("You did not have a tesseract irc config file set up. \nA default one has been made with the name \"IRC_Settings.tcfg\" \nPlease go ahead and configure it and restart the bot.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
