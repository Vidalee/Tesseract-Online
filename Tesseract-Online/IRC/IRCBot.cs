using Meebey.SmartIrc4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class IRCBot
    {
        // make an instance of the high-level API
        public static IrcClient irc = new IrcClient();

        // this method we will use to analyse queries (also known as private messages)
        public void OnQueryMessage(object sender, IrcEventArgs e)
        {
            switch (e.Data.MessageArray[0])
            {
                // debug stuff
                case "dump_channel":
                    string requested_channel = e.Data.MessageArray[1];
                    // getting the channel (via channel sync feature)
                    Channel channel = irc.GetChannel(requested_channel);

                    // here we send messages
                    irc.SendMessage(SendType.Message, e.Data.Nick, "<channel '" + requested_channel + "'>");

                    irc.SendMessage(SendType.Message, e.Data.Nick, "Name: '" + channel.Name + "'");
                    irc.SendMessage(SendType.Message, e.Data.Nick, "Topic: '" + channel.Topic + "'");
                    irc.SendMessage(SendType.Message, e.Data.Nick, "Mode: '" + channel.Mode + "'");
                    irc.SendMessage(SendType.Message, e.Data.Nick, "Key: '" + channel.Key + "'");
                    irc.SendMessage(SendType.Message, e.Data.Nick, "UserLimit: '" + channel.UserLimit + "'");

                    // here we go through all users of the channel and show their
                    // hashtable key and nickname 
                    string nickname_list = "";
                    nickname_list += "Users: ";
                    foreach (DictionaryEntry de in channel.Users)
                    {
                        string key = (string)de.Key;
                        ChannelUser channeluser = (ChannelUser)de.Value;
                        nickname_list += "(";
                        if (channeluser.IsOp)
                        {
                            nickname_list += "@";
                        }
                        if (channeluser.IsVoice)
                        {
                            nickname_list += "+";
                        }
                        nickname_list += ")" + key + " => " + channeluser.Nick + ", ";
                    }
                    irc.SendMessage(SendType.Message, e.Data.Nick, nickname_list);

                    irc.SendMessage(SendType.Message, e.Data.Nick, "</channel>");
                    break;
                case "gc":
                    GC.Collect();
                    break;
                // typical commands
                case "join":
                    irc.RfcJoin(e.Data.MessageArray[1]);
                    break;
                case "part":
                    irc.RfcPart(e.Data.MessageArray[1]);
                    break;
            }
        }

        // this method handles when we receive "ERROR" from the IRC server
        public void OnError(object sender, ErrorEventArgs e)
        {
            Logger.ERROR("Error: " + e.ErrorMessage);
        }

        //When an user disconnects.
        public void OnQuit(object sender, QuitEventArgs e)
        {
                Database.SetOnline(e.Who, false);
        }

        // this method will get all IRC messages
        public void OnRawMessage(object sender, IrcEventArgs e)
        {
            //Logger.IRC("Received: " + e.Data.RawMessage);

            Console.WriteLine("Received: " + e.Data.RawMessage);
        }

        public void Start()
        {

            MakeTConfig.CreateIRC();
            TConfigFile irc_config = new TConfigFile("IRC_Settings.tcfg");

            // UTF-8 test
            irc.Encoding = Encoding.UTF8;

            // wait time between messages, we can set this lower on own irc servers
            irc.SendDelay = 200;

            // we use channel sync, means we can use irc.GetChannel() and so on
            irc.ActiveChannelSyncing = true;

            // here we connect the events of the API to our written methods
            // most have own event handler types, because they ship different data
            irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            irc.OnError += new ErrorEventHandler(OnError);
            irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
            irc.OnQuit += new QuitEventHandler(OnQuit);


            string[] serverlist;
            // the server we want to connect to, could be also a simple string
            serverlist = new string[] { "irc.tesseract-game.net" };
            int port = 7000;
            string channel = "#general";
            try
            {
                // here we try to connect to the server and exceptions get handled
                irc.Connect(serverlist, port);
            }
            catch (ConnectionException e)
            {
                // something went wrong, the reason will be shown
                Logger.ERROR("Couldn't connect! Reason: " + e.Message);
            }

            try
            {
                // here we logon and register our nickname and so on 
                irc.Login("Xelia", "Xelia", 1, "Xelia", "Oui donc");
                irc.RfcOper(irc_config.Read("Oper username"), irc_config.Read("Oper password"));
                // join the channel
                irc.RfcJoin(channel);
                irc.RfcJoin("#admin");
                new IRCAuth(irc);

                // here we tell the IRC API to go into a receive mode, all events
                // will be triggered by _this_ thread (main thread in this case)
                // Listen() blocks by default, you can also use ListenOnce() if you
                // need that does one IRC operation and then returns, so you need then 
                // an own loop 
                irc.Listen();

                // when Listen() returns our IRC session is over, to be sure we call
                // disconnect manually
                irc.Disconnect();
            }
            catch (ConnectionException)
            {
                // this exception is handled because Disconnect() can throw a not
                // connected exception
            }
            catch (Exception e)
            {
                // this should not happen by just in case we handle it nicely
                System.Console.WriteLine("Error occurred! Message: " + e.Message);
                System.Console.WriteLine("Exception: " + e.StackTrace);
            }
        }

        public IRCBot()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Start();
            }).Start();
        }
    }
}
