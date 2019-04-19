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
            Room r = new Room("Aventuriers only !!");
            r.users.Add(new UserDTO() { username = "Vivi" });
            r.users.Add(new UserDTO() { username = "TurtleSmoke" });
            rm.AddRoom(r);
            Room r2 = new Room("Only pro players no noob pls");
            r2.users.Add(new UserDTO() { username = "DreamExe" });
            r2.users.Add(new UserDTO() { username = "e-Niem" });
            r2.users.Add(new UserDTO() { username = "un_random" });

            rm.AddRoom(r2);
            rm.ListRooms();
        }
    }
}
