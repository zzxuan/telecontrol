using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;  

namespace SocketHelp
{
    public delegate void Closedelegate(Socket socket);
    public delegate void ReciveDatadelegate(Socket socket,byte[] bytes);
    /// <summary>
    /// TCP数据管理类
    /// 数据组包方式：长度+数据
    /// </summary>
    public class TcpDataManager
    {
        public event Closedelegate CloseEvent;
        public event ReciveDatadelegate ReciveEvent;
        bool isdispose=false;
        Socket _Socket;
        public TcpDataManager(Socket socket)
        {
            _Socket = socket;
            Thread th = new Thread(new ParameterizedThreadStart(reciveClient));
            th.IsBackground = true;
            th.Start(_Socket);
        }
        /// <summary>
        /// 组包发送
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            try
            {
                if (isdispose)
                    return;
                uint len = (uint)(bytes.Length + 4);
                byte[] head = BitConverter.GetBytes(len);
                byte[] buf = head.Concat(bytes).ToArray();
                _Socket.Send(buf);
            }
            catch { }
        }


        /// <summary>
        /// 循环接收 并按包头拆分
        /// </summary>
        /// <param name="cilentsocket"></param>
        void reciveClient(object cilentsocket)
        {
            Thread.Sleep(10);
            Socket c = (Socket)cilentsocket;
            try
            {
                byte[] bytes = new byte[1024*1024];
                byte[] buf = new byte[0];
                uint len = 0;
                int num = 0;

                while (true)
                {
                    int n = c.Receive(bytes);
                    if (n == 0)
                        continue;

                    byte[] b = new byte[n];
                    Array.Copy(bytes, 0, b, 0, n);
                    buf = buf.Concat(b).ToArray();
                    num += n;
                    if (num > 4 && len == 0)
                    {
                        len = BitConverter.ToUInt32(buf, 0);
                    }
                    if (len > 0 && buf.Length >= len)
                    {
                        uint m = 0;
                        while (true)
                        {
                            byte[] data = new byte[len-4];
                            Array.Copy(buf, 4, data, 0, len-4);
                            
                            //--------------
                            if (ReciveEvent != null)
                            {
                                ReciveEvent(c, data);
                            }
                            //--------------
                            m += len;
                            if (buf.Length - m < len)
                            {
                                byte[] ss = new byte[buf.Length - m];
                                Array.Copy(buf, m, ss, 0, buf.Length - m);
                                buf = ss;
                                len = 0;
                                num = buf.Length;
                                break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //Logger.Trace(ex);
            }
            finally
            {
                if (CloseEvent != null)
                    CloseEvent(c);
                isdispose = true;
                c.Dispose();
            }
        }
    }
}
