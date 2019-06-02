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
            r.users.Add(new UserDTO() { username = "Joueur_A" });

            r.seed = 777;
            rm.AddRoom(r);

            rm.ListRooms();
        }
    }
}
