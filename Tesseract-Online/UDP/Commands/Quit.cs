using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Quit : Command
    {
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ep, "You are not authenticated !");
                return;
            }
            if (args[0] != "")
            {
                Main.rm.rooms.Where(r => r.code == args[0]).First().RemovePlayer(user);
            }
        }
    }
}
