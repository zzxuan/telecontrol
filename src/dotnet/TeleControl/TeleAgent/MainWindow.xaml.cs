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
using System.Diagnostics;
using System.Threading;

namespace TeleAgent
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
        public Process TeleServer;
        void Init()
        {
            this.Hide();
            var vvp = System.Diagnostics.Process.GetProcessesByName("TeleAgent");
            if (vvp != null && vvp.Length > 1)
            {
                MessageBox.Show("程序已启动~");
                Process.GetCurrentProcess().Kill();
                return;
            }

            var pArrayy = System.Diagnostics.Process.GetProcessesByName("TeleServer");
            if (pArrayy == null || pArrayy.Length == 0)
            {
                TeleServer = Process.Start(AppDomain.CurrentDomain.BaseDirectory + "TeleServer.exe");
            }
            else
                TeleServer = pArrayy[0];
            Thread.Sleep(1000);
            _Tray = new SysTray();
            _Tray.MainWin = this;
            Thread th = new Thread(new ThreadStart(() => 
            {
                while (true)
                {
                    try
                    {
                        if (TeleServer.HasExited)
                        {
                            TeleServer.Start();
                            _Tray.SetBalloonTip("服务已重启");
                        }
                    }
                    catch { }
                    Thread.Sleep(1000);
                }
            }));
            th.IsBackground = true;
            th.Start();
            if (AgentInfo.Instance.StartTime == 0)
            {
                //第一次启动
                AgentHelper.RunWhenStart(true, "TeleAgent", AppDomain.CurrentDomain.BaseDirectory + "TeleAgent.exe");
                AgentInfo.Instance.IsOpenStart = true;
            }
            AgentInfo.Instance.StartTime++;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                TeleServer.Kill();
                AgentInfo.Instance.Save(AppDomain.CurrentDomain.BaseDirectory + AgentInfo.filename);
            }
            catch { }
        }
    }
}
