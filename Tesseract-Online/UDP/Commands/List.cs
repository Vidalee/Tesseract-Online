using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class List : Command
    {
        public override void Trigger(string[] args, EndPoint ep, string trigger, TcpClient ns, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ns, "You are not authenticated !");
                return;
            }
            if (args.Length != 0) return;
            List<string> list = Main.rm.ListRooms();
            foreach (string rinfo in list)
            {
                UDPSocket.SendTo(ns, "RINFO " + rinfo);
                Thread.Sleep(75);
            }
        }
    }
}
