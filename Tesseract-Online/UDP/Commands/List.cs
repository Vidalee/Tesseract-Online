﻿using System;
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
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            if(user == null)
            {
                UDPSocket.SendTo(ep, "You are not authenticated !");
                return;
            }
            Thread.Sleep(50);
            foreach (string rinfo in Main.rm.ListRooms())
            {
                UDPSocket.SendTo(ep, "RINFO " + rinfo);
                Thread.Sleep(50);
            }
        }
    }
}
