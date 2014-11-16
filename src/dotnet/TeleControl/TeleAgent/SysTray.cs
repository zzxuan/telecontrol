using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TeleAgent
{
    class SysTray
    {
        public SysTray()
        {
            InitialTray();
        }
        MainWindow _MainWin;

        public MainWindow MainWin
        {
            get { return _MainWin; }
            set { _MainWin = value; }
        }

        private NotifyIcon notifyIcon;
        private void InitialTray()
        {
            //设置托盘的各个属性
            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "服务已启动";
            notifyIcon.Text = "遥控服务";
            Uri uri = new Uri("/TeleAgent;component/teleico.ico", UriKind.Relative);
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Application.GetResourceStream(uri).Stream);
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            MenuItem setting = new MenuItem("设置");
            setting.Click += new EventHandler(setting_Click);
            //帮助选项
            MenuItem help = new MenuItem("帮助");
            help.Click += new EventHandler(help_Click);
            //关于选项
            MenuItem about = new MenuItem("关于");
            about.Click += new EventHandler(about_Click);
            //退出菜单项
            MenuItem exit = new MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            MenuItem[] childen = new MenuItem[] { setting, help, about, exit };
            notifyIcon.ContextMenu = new ContextMenu(childen);

        }

        void about_Click(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipText = "联系作者:xiaoxuanfengzzx@163.com";
            notifyIcon.ShowBalloonTip(500);
        }

        void help_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            notifyIcon.BalloonTipText = "没啥要帮助的吧\n可以用客户端连接啦！！";
            notifyIcon.ShowBalloonTip(500);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void setting_Click(object sender, EventArgs e)
        {
            Settingwin set = new Settingwin();
            set.ShowDialog();
        }

        public void SetBalloonTip(string s)
        {
            notifyIcon.BalloonTipText = s;
            notifyIcon.ShowBalloonTip(500);
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //如果鼠标左键单击
            if (e.Button == MouseButtons.Left)
            {

            }
        }

        /// <summary>
        /// 退出选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();

            System.Windows.Application.Current.Shutdown();
        }
    }
}
