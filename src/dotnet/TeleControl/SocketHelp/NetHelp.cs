using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SocketHelp
{
    public class NetHelp
    {
        /// <summary>
        /// 获得本局IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ipstr="";
            if (readIPConfig(ref ipstr))
            {
                return ipstr;
            }
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// 返回主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        public static bool readIPConfig(ref string ip)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "IPConfig.ini";
            if (!File.Exists(filepath))
            {
                return false;
            }
            StreamReader sr = new StreamReader(filepath);
            string s = sr.ReadLine();
            string[] ss = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length > 1 && ss[1] == "1")
            {
                s = sr.ReadLine();
                ip = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                return true;
            }
            return false;
        }
    }
}
