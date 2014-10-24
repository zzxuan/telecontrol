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
        private IPAddress GroupAddress;
        private int GroupPort;
        UdpClient sender;
        IPEndPoint groupEP;
        public UdpSender(string ip, int port)
        {
            GroupAddress = IPAddress.Parse(ip);
            GroupPort = port;
            sender = new UdpClient();
            groupEP = new IPEndPoint(GroupAddress, GroupPort);
           
        }
        byte[] bys = new byte[1024];
        public void Send(byte[] bytes)
        {
            try
            {
                int i=0;

                for (; i < bytes.Length; i += bys.Length)
                {
                    Array.Copy(bytes, i, bys, 0, bys.Length);
                    sender.Send(bys, bys.Length, groupEP);
                }
                Array.Copy(bytes, i, bys, 0, bytes.Length-i);
                sender.Send(bys, bytes.Length - i, groupEP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void Close()
        {
            sender.Close();
        }
    }
}
