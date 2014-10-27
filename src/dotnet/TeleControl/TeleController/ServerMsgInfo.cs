using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace TeleController
{
    public class ServerMsgInfo
    {
        string _HostName;

        public string HostName
        {
            get { return _HostName; }
            set { _HostName = value; }
        }
        string _ServerIP;

        public string ServerIP
        {
            get { return _ServerIP; }
            set { _ServerIP = value; }
        }
        int _ServerPort;

        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }

        public string getJsonString()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);
        }
        public static ServerMsgInfo CreatFromJsonString(string jsonstr)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<ServerMsgInfo>(jsonstr);
        }
    }
}
