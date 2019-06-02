using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tesseract_Online
{
    class PInfo : Command
    {
        public override void Trigger(string[] args, EndPoint ep, string trigger, TcpClient ns, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ns, "You are not authenticated !");
                return;
            }
            if (Main.rm.rooms.Where(r => r.Contains(user)).Count() == 0)
            {
                Main.rm.rooms.First().AddPlayer(user);
            }
            if (args[0] != "")
            {
                Main.rm.rooms.Where(r => r.Contains(user)).First().Broadcast("PINFO " + user.gameId + " " + string.Join(" ", args), user);
                
            }
        }
    }
}
