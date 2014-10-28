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
        UdpListener listener;
        int _ClientPort = 26598;
        void Init()
        {
            listener = new UdpListener(NetHelp.GetIP(), _ClientPort);
            listener.StartListen();
            listener.UdpListenEvent += new UdpListener.UdpListenerdleteget(listener_UdpListenEvent);
            SendServerMsg();
        }
        void SendServerMsg()
        {
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
                    AddItem(js);
                    break;
            }
        }

        void AddItem(string js)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ServerMsgInfo info = ServerMsgInfo.CreatFromJsonString(js);
                Uri uri = new Uri("/TeleControl;component/Images/cmputer.png", UriKind.Relative);
                CustomListItem clist = new CustomListItem()
                {
                    IcoSource = new BitmapImage(uri),
                    ItemTitle = info.HostName,
                    ItemDesc = info.ServerIP + ":" + info.ServerPort,
                    Obj = info
                };
                clist.MouseDoubleClick += new MouseButtonEventHandler(clist_MouseDoubleClick);
                listBox1.Items.Add(clist);
            }));
        }

        void clist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomListItem clist = sender as CustomListItem;
            if (clist != null)
            {
                ServerMsgInfo info = clist.Obj as ServerMsgInfo;
                if (info != null)
                {
                    new CotrlWin(info).Show();
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Clear();
            SendServerMsg();
        }
    }
}
