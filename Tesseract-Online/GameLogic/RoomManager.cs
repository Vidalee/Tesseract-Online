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

        public void AddRoom(Room r)
        {
            rooms.Add(r);
        }

        public List<string> ListRooms()
        {
            List<string> list = new List<string>();
            foreach(Room room in rooms)
            {
                list.Add(room.ToUDPString());
            }
            return list;
        }
    }
}
