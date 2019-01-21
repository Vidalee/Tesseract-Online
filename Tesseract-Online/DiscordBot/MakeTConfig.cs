using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    public static class MakeIni
    {
        public static void Create()
        {
            //Checks to see if Bot_Settings.tcfg exists and if it doesnt it generates a default file for you to config and then ends the bot.
            if (!File.Exists("Bot_Settings.tcfg"))
            {
                string[] defaultData = { "Bot Token=token", "Bot Name = Xelia", "Status = status", "Prefix = !", "Channel = chId" };
                File.WriteAllLines("Bot_Settings.tcfg", defaultData);

                Console.WriteLine("You did not have a tesseract config file set up. \nA default one has been made with the name \"Bot_Settings.tcfg\" \nPlease go ahead and configure it and restart the bot.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
