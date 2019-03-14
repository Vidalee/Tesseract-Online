using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Room
    {
        private List<UserDTO> users = new List<UserDTO>();
        public string name;
        public Room()
        {
            name = Utils.RandomString(6);
        }

        public void AddPlayer(UserDTO user)
        {
            users.Add(user);
            IRCUtils.MakeJoin(user, name);
        }
    }
}
