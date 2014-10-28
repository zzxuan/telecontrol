using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SocketHelp;

namespace UdpTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                UdpSender.Send("225.0.0.12", 53535, Encoding.Default.GetBytes("sdaffffa"));
                Console.WriteLine("已发送");
                Thread.Sleep(500);
            }
        }
    }
}
