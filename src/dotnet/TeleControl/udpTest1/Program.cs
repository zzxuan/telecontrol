using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketHelp;
using System.Net.Sockets;
using System.Net;

namespace udpTest1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            UdpListener li = new UdpListener("225.0.0.12", 53535);
            li.StartListen();
            li.UdpListenEvent += new UdpListener.UdpListenerdleteget(li_UdpListenEvent);

            //var server = new UdpClient(53535);
            //var receivePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 53535);
            //while (true)
            //{
            //    byte[] recData = server.Receive(ref receivePoint);
            //    Console.WriteLine(Encoding.Default.GetString(recData));
            //}

            Console.Read();
        }

        static void li_UdpListenEvent(byte[] bytes, System.Net.IPEndPoint groupEP)
        {
            Console.WriteLine(Encoding.Default.GetString(bytes));
        }
    }
}
