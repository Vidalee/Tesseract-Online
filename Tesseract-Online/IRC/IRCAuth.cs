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
                    string password = sha256(e.Data.MessageArray[1] + "nyancat");
                    UserDTO user = new UserDTO()
                    {
                        authority = 0,
                        username = e.Data.Nick

                    };
                    if(true)
                    //if (Database.TryAuthentificate(e.Data.Nick, password, out user))
                    {
                        irc.SendMessage(SendType.Message, e.Data.Nick, "Successfully connected.");
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

        string sha256(string password)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private void ManageJoin(UserDTO user, string identity)
        {
            Database.SetOnline(identity, true);
            //TODO: load the channels from a config file
            irc.WriteLine("SAJOIN " + identity + " #general");
            irc.WriteLine("SAJOIN " + identity + " #announcements");
            irc.WriteLine("MODE #general +v " + identity);
            if (user.authority == 7) irc.WriteLine("SAJOIN " + identity + " #admin");
        }
    }
}
