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
        public static UDPSocket s = new UDPSocket();

        static void Main(string[] args)
        {
            Logger.Init();
            new Database();

            Console.WriteLine("Waiting 5 seconds to ensure the discord bot is logged in");
            Thread.Sleep(5000);
            IRCBot ircbot = new IRCBot();
            Main main = new Main();
            /*
            Example of how to auth
            UserDTO user;
            Console.WriteLine(Database.TryAuthentificate("oui", "ouioui", out user));
            */
            Console.WriteLine("IP Adress of the server?");
            s.Server(Console.ReadLine(), 27000);
            s.AddCommand("CONNECT", new Connect());
            s.AddCommand("JOIN", new Join());
            s.AddCommand("LIST", new List());
            s.AddCommand("PING", new Ping());
            s.AddCommand("QUIT", new Quit());
            s.AddCommand("CREATE", new Create());
            s.AddCommand("JINFO", new JInfo());
            s.AddCommand("PINFO", new PInfo());



            Console.ReadKey();


            Console.ReadKey();
            
        }
    }
}
