using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;  

namespace SocketHelp
{
    public class TcpServer
    {
        public event ReciveDatadelegate ReciveDataEvent;
        Socket s;
        Dictionary<Socket, TcpDataManager> _Clients = new Dictionary<Socket, TcpDataManager>();
        public TcpServer()
        {
        }
        
        public void Start(int port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket对象
            IPAddress serverIP = IPAddress.Parse(IPHelp.getIP());
            IPEndPoint server = new IPEndPoint(serverIP, port);    //实例化服务器的IP和端口
            s.Bind(server);
            s.Listen(100);
            Thread th = new Thread(new ThreadStart(content));
            th.IsBackground = true;
            th.Start();
        }

        void content()
        {
            while (true)
            {
                Socket temp = s.Accept();
                TcpDataManager dm=new TcpDataManager(temp);
                _Clients.Add(temp, dm);
                dm.CloseEvent += new Closedelegate(dm_CloseEvent);
                dm.ReciveEvent += new ReciveDatadelegate(dm_ReciveEvent);
            }
        }
        /// <summary>
        /// 给某个客户端发数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="bytes"></param>
        public void SendData(Socket socket, byte[] bytes)
        {
            if (_Clients.ContainsKey(socket))
            {
                _Clients[socket].SendData(bytes);
            }
        }
        /// <summary>
        /// 发送给所有客户端
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            foreach (var item in _Clients.Values)
            {
                item.SendData(bytes);
            }
        }

        void dm_ReciveEvent(Socket socket, byte[] bytes)
        {
            if (ReciveDataEvent != null)
                ReciveDataEvent(socket, bytes);
        }

        void dm_CloseEvent(Socket socket)
        {
            _Clients.Remove(socket);
        }
    }
}
