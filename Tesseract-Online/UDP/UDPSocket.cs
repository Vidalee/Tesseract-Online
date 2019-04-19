using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class UDPSocket
    {
        private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private static State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        private static Dictionary<EndPoint, UserDTO> users = new Dictionary<EndPoint, UserDTO>();
        private static Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public static void AddUser(EndPoint ep, UserDTO user)
        {
            if(!users.ContainsKey(ep))
                users.Add(ep, user);
        }

        public static void AddCommand(string command, Command cmd)
        {
            commands.Add(command, cmd);
        }

        public static void SendTo(EndPoint ep, string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            //IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), 27000); 
            _socket.BeginSendTo(data, 0, data.Length, SocketFlags.None, ep, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND {0}: {1}, {2}",ep.ToString(), bytes, msg);
            }, state);
        }

        public void Server(string ip, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            Console.WriteLine("NOW LISTENING ON " + _socket.LocalEndPoint.ToString());
            Receive();
        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }

        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));

                string[] msg = Encoding.ASCII.GetString(so.buffer, 0, bytes).Split(' ');

                string trigger = msg[0];
                if (commands.ContainsKey(trigger))
                {
                    if (users.ContainsKey(epFrom))
                        commands[trigger].Trigger(msg.Skip(1).ToArray(), epFrom, users[epFrom]);
                    else
                        commands[trigger].Trigger(msg.Skip(1).ToArray(), epFrom);
                }

            }, state);

        }
    }
}
