using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        private string last = "";
        private List<Tuple<string, EndPoint>> ToUse = new List<Tuple<string, EndPoint>>();

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public static void AddUser(EndPoint ep, UserDTO user)
        {
            if(!users.ContainsKey(ep))
                users.Add(ep, user);
        }

        public void AddCommand(string command, Command cmd)
        {
            commands.Add(command, cmd);
        }

        public static void SendTo(TcpClient client, string msg)
        {
            msg += ";";
            NetworkStream ns = client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(msg);
            Console.WriteLine("Sending back : " + msg);
            ns.Write(data, 0, data.Length);
        }

        public void Server(string ip, int port)
        {
            new Thread(() =>
            {
                //---listen at the specified IP and port no.---
                IPAddress localAdd = IPAddress.Any;
                TcpListener listener = new TcpListener(localAdd, port);
                Console.WriteLine("Listening...");
                listener.Start();
                while (true)
                {
                    //---incoming client connected---
                    TcpClient client = listener.AcceptTcpClient();
                    ListenTo(client);
                }
                listener.Stop();
                Console.ReadLine();
            }).Start();
        }

        private void ListenTo(TcpClient client)
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        //---get the incoming data through a network stream---
                        NetworkStream nwStream = client.GetStream();
                        byte[] buffer = new byte[client.ReceiveBufferSize];

                        //---read incoming stream---
                        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                        //---convert the data received into a string---
                        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        if (dataReceived == "") continue;
                        //ToUse.Add(new Tuple<string, EndPoint>(dataReceived, client.Client.RemoteEndPoint));

                        //---write back the text to the client---
                        string[] d = dataReceived.Split(';');
                        foreach (string m in d)
                        {
                            if (m == "") continue;
                            Console.WriteLine("Received : " + m);

                            string[] msg = m.Split(' ');

                            string trigger = msg[0];
                            if (commands.ContainsKey(trigger))
                            {
                                if (users.ContainsKey(client.Client.RemoteEndPoint))
                                    commands[trigger].Trigger(msg.Skip(1).ToArray(), client.Client.RemoteEndPoint, trigger, client, users[client.Client.RemoteEndPoint]);
                                else
                                    commands[trigger].Trigger(msg.Skip(1).ToArray(), client.Client.RemoteEndPoint, trigger, client);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }).Start();
        }
    }
}
