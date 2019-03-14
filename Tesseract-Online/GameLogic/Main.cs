using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Main
    {
        public static RoomManager rm = new RoomManager();

        public Main()
        {
            rm.AddRoom();
            rm.AddRoom();
            rm.ListRooms();
        }
    }
}
