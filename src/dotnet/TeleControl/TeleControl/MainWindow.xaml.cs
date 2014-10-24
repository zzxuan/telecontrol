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
        TcpClient client;
        void Init()
        {
            client = new TcpClient(NetHelp.getIP(), 26598);
            client.ConnectServer();
            client.ReciveEvent += new ReciveDatadelegate(client_ReciveEvent);
            TeleMouseContrl m = new TeleMouseContrl() { X = 0.5f, Y = 0.5f, TeleMouseEvent = TeleMouseEventEnum.Move };
            client.SendData(m.ToBytes());
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
