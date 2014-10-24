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
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using SocketHelp;
using ScreenHelp;
using System.Threading;
using TeleController;

namespace TeleServer
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

        SysTray _Tray;
        //UdpSender sender;
        TcpServer server;
        void Init()
        {
            Hide();
            _Tray = new SysTray();
            //sender = new UdpSender(NetHelp.getIP(), 9998);
            server = new TcpServer();
            server.Start(26598);
            server.ReciveDataEvent += new ReciveDatadelegate(server_ReciveDataEvent);
            Thread th = new Thread(new ThreadStart(SendScreen));
            th.IsBackground = true;
            th.Start();
        }

        void server_ReciveDataEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        {
            try
            {
                switch (bytes[0])
                {
                    case TeleContans.CmdMouse:
                        TeleMouseContrl mouse = new TeleMouseContrl();
                        mouse.FromBytes(bytes);
                        mouse.SetMouseEvent();
                        break;
                }
            }
            catch
            { }
        }

        void SendScreen()
        {
            while (true)
            {
                //ScreenShot.getBitmapBites();
                server.SendData(ScreenShot.getScreenBites());
                Thread.Sleep(50);
            }
        }
    }
}
