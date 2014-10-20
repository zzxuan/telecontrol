using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using ServerDemo;

namespace ServerDemo.ServerHelp
{
    public delegate void Datadelegate(string s);
    /// <summary>
    /// 接口实现类，这里主要用来访问数据库
    /// </summary>
    public class DataService : IDataBaseService
    {
        public static Datadelegate GetMsgEvent;
        /// <summary>
        /// 接口实现类
        /// </summary>
        public string GetMsg(string msg)
        {
            if (GetMsgEvent != null)
                GetMsgEvent(msg);
            return "hello wpf wcf Rest";
        }
    }
}
