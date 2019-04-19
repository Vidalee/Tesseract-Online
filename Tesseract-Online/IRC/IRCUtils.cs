using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class IRCUtils
    {
        public static void MakeJoin(UserDTO user, string channel)
        {
            IRCBot.irc.WriteLine("SAJOIN " + user.username + " #" + channel);
        }

        public static void MakeQuit(UserDTO user, string channel)
        {
            IRCBot.irc.WriteLine("SAPART " + user.username + " #" + channel);
        }
    }
}
