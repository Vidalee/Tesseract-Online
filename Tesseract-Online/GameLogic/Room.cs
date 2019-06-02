using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Room
    {
        public List<UserDTO> users = new List<UserDTO>();
        public string name;
        public string code;
        public int seed;
        long start = 0;
        public Room(string name)
        {
            this.name = name;
            code = Utils.RandomString(6);
        }

        public void AddPlayer(UserDTO user)
        {
            users.RemoveAll(u => u.username == user.username);
            user.gameId = users.Count() + 1;
            users.Add(user);
            UDPSocket.SendTo(user.client, "SET seed " + seed);
            Thread.Sleep(25);
            UpdateIds();
            Broadcast("SPAWN " + users.Count(), user);
            for(int i =0; i < users.Count(); i++)
            {
                if (users[i].client != null)
                { 
                    UDPSocket.SendTo(user.client, "SPAWN " + (i + 1));
                    Thread.Sleep(25);
                }
            }
            IRCUtils.MakeJoin(user, code);
        }

        public bool Contains(UserDTO user)
        {
            return users.Where(u => u.username == user.username).Count() != 0;
        }

        public void UpdateIds()
        {
            for(int i = 0; i < users.Count(); i++)
            {
                //Might be null if it's a fake user for demonstration purposes
                if(users[i].endpoint != null)
                    UDPSocket.SendTo(users[i].client, "SET id " + (i + 1));
            }
        }

        public void RemovePlayer(UserDTO user)
        {
            if (users.Where(u => u.username == user.username).Count() > 0)
            {
                users.Remove(users.Where(u => u.username == user.username).First());
                IRCUtils.MakeQuit(user, code);
            }
            UpdateIds();
        }

        public void DeadClient(EndPoint ep)
        {
           // RemovePlayer(users.Where(u => u.endpoint == ep).First());
        }

        public string ToUDPString()
        {
            return code + " " + users.Count + " " + string.Join(" ", users.Select(u => u.username)) + " " + name;
        }

        public void Broadcast(string msg, UserDTO user)
        {
            Thread.Sleep(10);
            foreach (UserDTO u in users.Where(u => u.username != user.username))
                if (u.client != null)
                    UDPSocket.SendTo(u.client, msg);
            if (msg.Contains("START"))
            {
                Thread.Sleep(4000);
                StartGame();
            }
            if (msg.Contains("STOP"))
            {
                StopGame();
            }
        }

        private void StartGame()
        {
            Console.WriteLine("Game started !");
            start = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            foreach (UserDTO u in users)
            {
                if (u.client == null) continue;
                for (int i = 0; i < users.Count(); i++)
                {
                    if (users[i].client != null)
                    {
                        UDPSocket.SendTo(u.client, "SPAWN " + (i + 1));
                        Thread.Sleep(25);
                    }
                }
            }
        }

        private void StopGame()
        {
            long stop = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            long time = stop - start;
            Database.SetScore(time, stop, GetUsername(0), GetUsername(1), GetUsername(2), GetUsername(3), seed);
        }

        private string GetUsername(int id)
        {
            if (users.Count > id)
                return users[id].username;
            return "";
        }
    }
}
