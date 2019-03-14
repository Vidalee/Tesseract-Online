using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    public abstract class Command
    {
        public abstract void Trigger(string[] args, EndPoint ep, UserDTO user = null);
    }
}
