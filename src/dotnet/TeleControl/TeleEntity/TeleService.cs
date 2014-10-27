using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketHelp;
using System.Net.Sockets;
using System.Threading;
using ScreenHelp;

namespace TeleController
{
    public class TeleService
    {
        private static TeleService _InstanceService = new TeleService();
        /// <summary>
        /// 服务单例
        /// </summary>
        public static TeleService InstanceService
        {
            get { return TeleService._InstanceService; }
            //set { TeleService._InstanceService = value; }
        }

        private TeleService() { }
        int _serverPort = 54322;

        TcpServerHelp _Server;

        UdpListener _udpListen;

        List<Socket> Controls = new List<Socket>();
        TeleMouseContrl mouse = new TeleMouseContrl();
        public void Init()
        {
            _udpListen = new UdpListener(TeleContans.UdpIP, TeleContans.UdpPort);
            _udpListen.StartListen();
            _udpListen.UdpListenEvent += new UdpListener.UdpListenerdleteget(_udpListen_UdpListenEvent);

            _Server = new TcpServerHelp();
            _Server.Start(_serverPort);
            _Server.ReciveDataEvent += new ReciveDatadelegate(_Server_ReciveDataEvent);
            _Server.CloseSocketEvent += new Closedelegate(_Server_CloseSocketEvent);

            Thread th = new Thread(new ThreadStart(SendScreen));
            th.IsBackground = true;
            th.Start();
        }

        void _Server_CloseSocketEvent(Socket socket)
        {
            lock (Controls)
                if (Controls.Contains(socket))
                    Controls.Remove(socket);
        }

        void SendScreen()
        {
            while (true)
            {
                if (Controls.Count > 0)
                {
                        foreach (var socket in Controls)
                            _Server.SendData(socket,ScreenShot.getScreenBites());
                }
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// TCP 组播接收 用于远程控制
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="bytes"></param>
        void _Server_ReciveDataEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        {
            switch (bytes[0])
            {
                case TeleContans.CmdStart://客户端问答
                    {
                        lock (Controls)
                            if (!Controls.Contains(socket))
                                Controls.Add(socket);
                    }
                    break;
                case TeleContans.CmdStop:
                    {
                        lock (Controls)
                            if (Controls.Contains(socket))
                                Controls.Remove(socket);
                    }
                    break;
                case TeleContans.CmdMouse:
                    {
                        mouse.FromBytes(bytes);
                        mouse.SetMouseEvent();
                    }
                    break;
            }
        }

        /// <summary>
        /// Udp 组播接收
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="groupEP"></param>
        void _udpListen_UdpListenEvent(byte[] bytes, System.Net.IPEndPoint groupEP)
        {
            string clientIP = groupEP.Address.ToString();
            switch (bytes[0])
            {
                case TeleContans.MsgClientAsk://客户端问答
                    {
                        int clietport = BitConverter.ToInt32(bytes, 1);
                        SendServerMsg(clientIP, clietport);
                    }
                    break;
            }
        }

        /// <summary>
        /// 发送服务端信息
        /// </summary>
        void SendServerMsg(string ip,int port)
        {
            string serverIP = NetHelp.GetIP();
            string hostname = NetHelp.GetHostName();
            ServerMsgInfo ser = new ServerMsgInfo() { HostName = hostname, ServerIP = serverIP, ServerPort = _serverPort };
            byte[] bs = new byte[] {TeleContans.MsgString };
            byte[] bb = Encoding.UTF8.GetBytes(ser.getJsonString());
            bs = bs.Concat(bb).ToArray();
            UdpSender.Send(ip, port, bs);
        }
    }
}
