using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init();
            new Database();

            Console.WriteLine("Waiting 5 seconds to ensure the discord bot is logged in");
            Thread.Sleep(5000);
            IRCBot ircbot = new IRCBot();

            /*
            Example of how to auth
            UserDTO user;
            Console.WriteLine(Database.TryAuthentificate("oui", "ouioui", out user));
            */
            Console.ReadKey();
            
        }
    }
}
