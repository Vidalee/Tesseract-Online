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
           /* //IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), 27000);
            try
            {
                _socket.SendTo(data, 0, data.Length, SocketFlags.None, ep);
                Console.WriteLine("send: " + msg);
            }
            catch
            {
                /*
                   Console.WriteLine("dead client :(");
                if (Main.rm.rooms.Where(r => r.users.Where(u => u.endpoint == ep).Count() > 0).Count() > 0)
                {
                    Main.rm.rooms.Where(r => r.users.Where(u => u.endpoint == ep).Count() > 0).First().DeadClient(ep);
                }
                */
            //}
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
            }).Start();
        }
        public void Servera(string ip, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            Console.WriteLine("NOW LISTENING ON " + _socket.LocalEndPoint.ToString());
            Receive();

           

            /*new Thread(() =>
            {
                while (true)
                {
                    if (ToUse.Count != 0)
                    {
                        Tuple<string, EndPoint> tuple = ToUse[0];
                        ToUse.RemoveAll(s => s.Item1 == tuple.Item1);
                        if (tuple.Item1 == last || tuple.Item1 == "JOIN")
                        {
                            Console.WriteLine("doublon !!");

                        }
                        else
                        {
                            last = tuple.Item1;
                            Console.WriteLine("RECV: {0}: {1}", tuple.Item2.ToString(), tuple.Item1);

                            string[] msg = tuple.Item1.Split(' ');

                            string trigger = msg[0];
                            if (commands.ContainsKey(trigger))
                            {
                                if (users.ContainsKey(epFrom))
                                    commands[trigger].Trigger(msg.Skip(1).ToArray(), tuple.Item2, trigger, users[tuple.Item2]);
                                else
                                    commands[trigger].Trigger(msg.Skip(1).ToArray(), tuple.Item2, trigger);
                            }
                        }
                    }
                }
            }).Start();*/

        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Send(string text)
        {
            Thread.Sleep(5);
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
                string t = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                ToUse.Add(new Tuple<string, EndPoint>(t, epFrom));
            }, state);

        }
    }
}
