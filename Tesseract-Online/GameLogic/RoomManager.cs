using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class RoomManager
    {
        public List<Room> rooms = new List<Room>();

        public RoomManager()
        {

        }

        public void AddRoom()
        {
            rooms.Add(new Room());
        }

        public string ListRooms()
        {
            string res = "";
            foreach(Room room in rooms)
            {
                res += room.name + " ";
            }
            return res;
        }
    }
}
