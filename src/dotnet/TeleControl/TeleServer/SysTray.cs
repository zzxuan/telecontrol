using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TeleServer
{
    class SysTray
    {
        public SysTray()
        {
            InitialTray();
        }
        private NotifyIcon notifyIcon;
        private void InitialTray()
        {
            //设置托盘的各个属性
            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "服务已启动";
            notifyIcon.Text = "遥控服务";
            Uri uri = new Uri("/TeleServer;component/Images/remcotrl.ico", UriKind.Relative);
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Application.GetResourceStream(uri).Stream);
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            MenuItem setting1 = new MenuItem("setting1");
            MenuItem setting2 = new MenuItem("setting2");
            MenuItem setting = new MenuItem("setting", new MenuItem[] { setting1, setting2 });

            //帮助选项
            MenuItem help = new MenuItem("help");

            //关于选项
            MenuItem about = new MenuItem("about");

            //退出菜单项
            MenuItem exit = new MenuItem("exit");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            MenuItem[] childen = new MenuItem[] { setting, help, about, exit };
            notifyIcon.ContextMenu = new ContextMenu(childen);

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
