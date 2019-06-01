using System.Linq;
using System.Net;

namespace Tesseract_Online
{
    class Join : Command
    {
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ep, "You are not authenticated !");
                return;
            }
            if (args[0] != "") Main.rm.rooms.First().AddPlayer(user);
                //Main.rm.rooms.Where(r => r.code == args[0]).First().AddPlayer(user);
        }
    }
}
