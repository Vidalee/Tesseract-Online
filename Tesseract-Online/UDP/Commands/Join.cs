using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tesseract_Online
{
    class Join : Command
    {
        public override void Trigger(string[] args, EndPoint ep, string trigger, TcpClient ns, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ns, "You are not authenticated !");
                return;
            }
            if (args[0] != "") Main.rm.rooms.First().AddPlayer(user);
                //Main.rm.rooms.Where(r => r.code == args[0]).First().AddPlayer(user);
        }
    }
}
