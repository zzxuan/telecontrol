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
using Microsoft.Win32;

namespace TeleAgent
{
    /// <summary>
    /// Settingwin.xaml 的交互逻辑
    /// </summary>
    public partial class Settingwin : Window
    {
        public Settingwin()
        {
            InitializeComponent();
            checkBox1.IsChecked = AgentInfo.Instance.IsOpenStart;
        }




        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            bool isstart = (bool)((CheckBox)sender).IsChecked;
            AgentHelper.RunWhenStart(isstart, "TeleAgent", AppDomain.CurrentDomain.BaseDirectory + "TeleAgent.exe");
            AgentInfo.Instance.IsOpenStart = isstart;
        }
    }
}
