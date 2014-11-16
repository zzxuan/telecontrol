using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TeleAgent
{
    class AgentHelper
    {
        /// <summary>         
        /// 开机启动项        
        /// </summary>       
        /// <param name="Started">是否启动</param>         
        /// <param name="name">启动值的名称</param>          
        /// <param name="path">启动程序的路径</param>         
        public static void RunWhenStart(bool Started, string name, string path)
        {
            try
            {
                RegistryKey HKLM = Registry.LocalMachine;
                RegistryKey Run;
                try
                {
                    Run = HKLM.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//打开注册表子项 
                }
                catch
                {
                    Run = HKLM.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                if (Started == true)
                {
                    try
                    {
                        Run.SetValue(name, path);
                        HKLM.Close();
                    }
                    catch//没有权限会异常            
                    { }
                }
                else
                {
                    try
                    {
                        Run.DeleteValue(name);
                        HKLM.Close();
                    }
                    catch//没有权限会异常 
                    { }
                }
            }
            catch { }
        }
    }
}
