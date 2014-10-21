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
            client = new TcpClient(IPHelp.getIP(), 20011);
            client.ConnectServer();
            client.ReciveEvent += new ReciveDatadelegate(client_ReciveEvent);
            Thread th = new Thread(new ThreadStart(showImage)) { IsBackground=true};
            th.Start();
            
        }
       Stack<byte[]> datas=new Stack<byte[]>();
        void client_ReciveEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        {
            datas.Clear();
            datas.Push(bytes);
        }
        void showImage()
        {
            while (true)
            {
                if (datas.Count > 0)
                {
                    byte[] bytes = datas.Pop();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        image1.Source = BitmapHelp.BytestoBitmapImage(bytes);
                    }));
                }
                Thread.Sleep(20);
            }
        }

    }
}
