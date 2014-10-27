using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SocketHelp;
using ScreenHelp;
using System.Threading;
using System.Runtime.InteropServices;
using TeleController;

namespace TeleControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        //TcpClientHelp client;
        //void Init()
        //{
        //    client = new TcpClientHelp(NetHelp.GetIP(), 26598);
        //    client.ConnectServer();
        //    client.ReciveEvent += new ReciveDatadelegate(client_ReciveEvent);
        //    TeleMouseContrl m = new TeleMouseContrl() { X = 0.5f, Y = 0.5f, TeleMouseEvent = TeleMouseEventEnum.Move };
        //    client.SendData(m.ToBytes());
        //}
        //void client_ReciveEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        //{
        //    this.Dispatcher.Invoke(new Action(() =>
        //    {
        //        image1.Source = BitmapHelp.BytestoBitmapImage(bytes);
        //    }));
        //}

        UdpListener listener;
        int _ClientPort = 26598;
        void Init()
        {
            listener = new UdpListener(NetHelp.GetIP(), _ClientPort);
            listener.StartListen();
            listener.UdpListenEvent += new UdpListener.UdpListenerdleteget(listener_UdpListenEvent);
            byte[] cport = BitConverter.GetBytes(_ClientPort);
            byte[] b = new byte[] { TeleContans.MsgClientAsk };
            UdpSender.Send(TeleContans.UdpIP, TeleContans.UdpPort, b.Concat(cport).ToArray());
        }

        void listener_UdpListenEvent(byte[] bytes, System.Net.IPEndPoint groupEP)
        {
            byte[] buf = new byte[bytes.Length - 1];
            Array.Copy(bytes, 1, buf, 0, buf.Length);
            switch (bytes[0])
            {
                case TeleContans.MsgString:
                    string js = Encoding.UTF8.GetString(buf);
                    this.Dispatcher.Invoke(new Action(() => {
                        listBox1.Items.Add(js);
                    }));
                    
                    break;
            }
        }
        TcpClientHelp client;
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(client!=null)
            {
                client.Close();
            }
            string ss = listBox1.SelectedItem.ToString();
            ServerMsgInfo info = ServerMsgInfo.CreatFromJsonString(ss);
            client = new TcpClientHelp(info.ServerIP, info.ServerPort);
            client.ConnectServer();
            client.ReciveEvent += new ReciveDatadelegate(client_ReciveEvent);
            client.SendData(new byte[] {TeleContans.CmdStart });
        }

        void client_ReciveEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                image1.Source = BitmapHelp.BytestoBitmapImage(bytes);
            }));
        }


    }
}
