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
        void Init()
        {
            Hide();
            _Tray = new SysTray();
            //sender = new UdpSender(NetHelp.getIP(), 9998);
            TeleService.InstanceService.Init();
        }

    }
}
