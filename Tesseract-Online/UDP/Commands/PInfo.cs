using System.Linq;
using System.Net;

namespace Tesseract_Online
{
    class PInfo : Command
    {
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ep, "You are not authenticated !");
                return;
            }
            if (Main.rm.rooms.Where(r => r.Contains(user)).Count() == 0)
            {
                Main.rm.rooms.First().AddPlayer(user);
            }
            if (args[0] != "")
            {
                Main.rm.rooms.Where(r => r.Contains(user)).First().Broadcast("PINFO " + user.gameId + " " + string.Join(" ", args), user);
            }
        }
    }
}
