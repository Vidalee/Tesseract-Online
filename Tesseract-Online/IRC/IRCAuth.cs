using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class IRCAuth
    {
        private IrcClient irc;

        private Dictionary<string, UserDTO> CurrentUsers;

        public IRCAuth(IrcClient irc)
        {
            CurrentUsers = new Dictionary<string, UserDTO>();
            this.irc = irc;
            irc.OnJoin += new JoinEventHandler(OnJoin);
            irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
        }

        private void OnJoin(object sender, IrcEventArgs e)
        {
            if (Database.TryGetUser(e.Data.Nick, out UserDTO user))
            {
                switch (user.authority)
                {
                    case 7:
                        irc.Op(e.Data.Channel, e.Data.Nick);
                        break;
                    case 5:
                        irc.WriteLine("MODE " + e.Data.Channel + " +h " + e.Data.Nick);
                        break;
                }
            }
            else
                Logger.IRC(e.Data.Nick + " joined an irc channel without having an account on Tesseract !");
        }
        private void OnQueryMessage(object sender, IrcEventArgs e)
        {
            switch (e.Data.MessageArray[0].ToLower())
            {
                case "identify":
                    string password = e.Data.MessageArray[1];
                    UserDTO user;
                    if (Database.TryAuthentificate(e.Data.Nick, password, out user))
                    {
                        irc.SendMessage(SendType.Message, e.Data.Nick, ">Correct password!");
                        Logger.IRC(e.Data.Nick + " Just Connected!");
                        ManageJoin(user, e.Data.Nick);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, e.Data.Nick, ">Wrong password for " + e.Data.Nick + "!");
                    }
                    break;
            }
        }

        private void ManageJoin(UserDTO user, string identity)
        {
            //TODO: load the channels from a config file
            irc.WriteLine("SAJOIN " + identity + " #general");
            irc.WriteLine("SAJOIN " + identity + " #announcements");
            irc.WriteLine("MODE #general +v " + identity);
        }
    }
}
