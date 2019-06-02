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
        public override void Trigger(string[] args, EndPoint ep, string trigger, TcpClient ns, UserDTO user = null)
        {
            Console.WriteLine(args.Length);
            if (args.Length < 2) return;
            //CONNECT user pass
            if (Database.TryAuthentificate(args[0], sha256(args[1] + "nyancat"), out user))
            {
                Console.WriteLine("Joueur " + user.username + " connecté !");
                user.endpoint = ep;
                user.client = ns;
                UDPSocket.AddUser(ep, user);
                UDPSocket.SendTo(ns, "CPASS");
            }
            else
            {
                Console.WriteLine("Mauvais mot de passe pour " + args[0]);
                UDPSocket.SendTo(ns, "WPASS");
            }
        }

        string sha256(string password)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
