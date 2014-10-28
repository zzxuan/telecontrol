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
using System.Windows.Shapes;
using TeleController;
using SocketHelp;
using ScreenHelp;

namespace TeleControl
{
    /// <summary>
    /// CotrlWin.xaml 的交互逻辑
    /// </summary>
    public partial class CotrlWin : Window
    {
        private TcpClientHelp client;
        public CotrlWin(ServerMsgInfo info)
        {
            InitializeComponent();

            client = new TcpClientHelp(info.ServerIP, info.ServerPort);
            if (client.ConnectServer())
            {
                client.ReciveEvent += new ReciveDatadelegate(client_ReciveEvent);
                client.SendData(new byte[] { TeleContans.CmdStart });
                Logger.Trace("开始");
            }
        }

        void client_ReciveEvent(System.Net.Sockets.Socket socket, byte[] bytes)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                image1.Source = BitmapHelp.BytestoBitmapImage(bytes);
            }));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (client != null)
                client.Close();
        }
        struct MouseDataStruct
        {
            public float x;
            public float y;
        }
        MouseDataStruct GetMouseData()
        {
            double w = image1.ActualWidth;
            double h = image1.ActualHeight;
            Point p = Mouse.GetPosition(image1);
            float x = (float)(p.X / w);
            float y = (float)(p.Y / h);
            return new MouseDataStruct() { x = x, y = y };
        }

        void SendMouseCmd(TeleMouseEventEnum mouseState)
        {
            if (client == null)
                return;
            MouseDataStruct md = GetMouseData();
            TeleMouseContrl tm = new TeleMouseContrl() { X = md.x, Y = md.y, TeleMouseEvent = mouseState };
            TeleMouseContrl tm1 = new TeleMouseContrl() { X = md.x, Y = md.y, TeleMouseEvent = TeleMouseEventEnum.Move };
            client.SendData(tm1.ToBytes());
            client.SendData(tm.ToBytes());
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            MouseDataStruct md = GetMouseData();
            TeleMouseContrl tm = new TeleMouseContrl() { X = md.x, Y = md.y};
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SendMouseCmd(TeleMouseEventEnum.LeftDown);
            }
            else
            {
                SendMouseCmd(TeleMouseEventEnum.RightDown);
            }
            
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SendMouseCmd(TeleMouseEventEnum.LeftUp);
        }

        private void image1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SendMouseCmd(TeleMouseEventEnum.RightUp);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                SendMouseCmd(TeleMouseEventEnum.Move);
            }
        }
    }
}
