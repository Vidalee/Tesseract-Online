using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Room
    {
        public List<UserDTO> users = new List<UserDTO>();
        public string name;
        public string code;
        public int seed;
        public Room(string name)
        {
            this.name = name;
            code = Utils.RandomString(6);
        }

        public void AddPlayer(UserDTO user)
        {
            users.Add(user);
            IRCUtils.MakeJoin(user, code);
        }

        public void RemovePlayer(UserDTO user)
        {
            if (users.Where(u => u.username == user.username).Count() > 0)
            {
                users.Remove(users.Where(u => u.username == user.username).First());
                IRCUtils.MakeQuit(user, code);
            }
        }

        public string ToUDPString()
        {
            return code + " " + users.Count + " " + string.Join(" ", users.Select(u => u.username)) + " " + name;
        }
    }
}
