using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Connect : Command
    {
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            //CONNECT user pass
            if (Database.TryAuthentificate(args[0], args[1], out user))
            {
                Console.WriteLine("Joueur " + user.username + " connecté !");
                user.endpoint = ep;
                UDPSocket.AddUser(ep, user);
                UDPSocket.SendTo(ep, "Correct password");
            }
            else
            {
                Console.WriteLine("Mauvais mot de passe pour " + args[0]);
                UDPSocket.SendTo(ep, "Wrong password");
            }
        }
    }
}
