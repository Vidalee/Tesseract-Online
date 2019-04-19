using System.Net;

namespace Tesseract_Online
{
    class Create : Command
    {
        public override void Trigger(string[] args, EndPoint ep, UserDTO user = null)
        {
            if (user == null)
            {
                UDPSocket.SendTo(ep, "You are not authenticated !");
                return;
            }
            string name = "";
            for (int i = 1; i < args.Length; i++) name += args[i] + " ";
            Room r = new Room(name);
            r.seed = int.Parse(args[0]);
            Main.rm.AddRoom(r);
        }
    }
}
