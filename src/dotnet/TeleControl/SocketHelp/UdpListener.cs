﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketHelp
{
    public class UdpListener
    {
        public delegate void UdpListenerdleteget(byte[] bytes, IPEndPoint groupEP);
        public event UdpListenerdleteget UdpListenEvent;
        // 设置IP，IPV6
        private  IPAddress GroupAddress;
        // 设置端口
        private int GroupPort;
        UdpClient listener;

        public UdpListener(string ip, int port)
        {
            GroupAddress = IPAddress.Parse(ip);
            GroupPort = port;
            listener = new UdpClient(GroupPort);
            try
            {
                //组播
                listener.JoinMulticastGroup(GroupAddress);
            }
            catch { }
        }
        Thread th;
        bool isStart = false;
        public void StartListen()
        {
            if (isStart)
                return;
            th = new Thread(new ThreadStart(StartListener));
            th.IsBackground = true;
            th.Start();
            isStart = true;
        }
        public void StopListen()
        {
            try 
            {
                if (th.IsAlive)
                    th.Abort();
                listener.Close();
                
                isStart = false;
            }
            catch { }
        }

        void StartListener()
        {
            IPEndPoint groupEP = new IPEndPoint(GroupAddress, GroupPort);
            //listener.Connect(groupEP);
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                if (UdpListenEvent != null)
                    UdpListenEvent(bytes, groupEP);
            }
        }
    }
}
