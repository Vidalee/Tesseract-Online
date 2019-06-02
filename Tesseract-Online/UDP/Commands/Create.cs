using System.Net;
using System.Net.Sockets;

namespace Tesseract_Online
{
    class Create : Command
    {
        public override void Trigger(string[] args, EndPoint ep, string trigger, TcpClient ns, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ns, "You are not authenticated !");
                return;
            }
            string name = "";
            for (int i = 1; i < args.Length; i++) name += args[i] + " ";
            Room r = new Room(name);
            r.seed = int.Parse(args[0]);
            Main.rm.AddRoom(r);
        }
    }
}
