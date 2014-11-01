using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketHelp
{
    public class UdpSender
    {
        public static void Send(string ip, int port, byte[] bytes)
        {
            UdpSender sen = new UdpSender(ip, port);
            sen.Send(bytes);
            sen.Close();
        }

        private IPAddress GroupAddress;
        private int GroupPort;
        UdpClient sender;
        IPEndPoint groupEP;
        public UdpSender(string ip, int port)
        {
            try
            {
                GroupAddress = IPAddress.Parse(ip);
                GroupPort = port;
                sender = new UdpClient();
                groupEP = new IPEndPoint(GroupAddress, GroupPort);
            }
            catch { }
        }

        public void Send(byte[] bytes)
        {
            sender.Send(bytes,bytes.Length,groupEP);
        }
        public void Close()
        {
            sender.Close();
        }
    }
}
