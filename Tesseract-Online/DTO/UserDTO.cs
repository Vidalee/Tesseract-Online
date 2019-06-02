using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tesseract_Online
{
  
    public class UserDTO
    {
        public string username;
        public string password;
        public int authority;
        public EndPoint endpoint;
        public int gameId;
        public TcpClient client;
    }
}
