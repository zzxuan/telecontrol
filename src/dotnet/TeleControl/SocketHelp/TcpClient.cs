using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketHelp
{
    public class TcpClientHelp
    {
        public event Closedelegate CloseEvent;
        public event ReciveDatadelegate ReciveEvent;
        Socket _Socket;
        IPEndPoint _Ipe;
        TcpDataManager _Dm;
        public TcpClientHelp(string ip, int port)
        {
            IPAddress ipadd = IPAddress.Parse(ip);
            _Ipe = new IPEndPoint(ipadd, port);//把ip和端口转化为IPEndpoint实例 
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket 
        }

        public bool ConnectServer()
        {
            try 
            {
                _Socket.Connect(_Ipe);
                _Dm = new TcpDataManager(_Socket);
                _Dm.ReciveEvent += new ReciveDatadelegate(_Dm_ReciveEvent);
                _Dm.CloseEvent += new Closedelegate(_Dm_CloseEvent);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public void Close()
        {
            _Socket.Close();
            _Socket.Dispose();
        }

        void _Dm_CloseEvent(Socket socket)
        {
            if (CloseEvent != null)
            {
                CloseEvent(socket);
            }
        }

        void _Dm_ReciveEvent(Socket socket, byte[] bytes)
        {
            if (ReciveEvent != null)
            {
                ReciveEvent(socket,bytes);
            }
        }

        public void SendData(byte[] bytes)
        {
            _Dm.SendData(bytes);
        }

    }
}
