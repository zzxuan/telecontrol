using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace TeleAgent
{
    class AgentInfo
    {
        public const string filename = "teleagent.info";
        static AgentInfo _Instance = CreatFromFile(AppDomain.CurrentDomain.BaseDirectory + filename);

        internal static AgentInfo Instance
        {
            get { return AgentInfo._Instance; }
        }
        int _StartTime = 0;

        public int StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        bool _IsOpenStart=false;

        public bool IsOpenStart
        {
            get { return _IsOpenStart; }
            set { _IsOpenStart = value; }
        }

        public string getJsonString()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);
        }
        public static AgentInfo CreatFromJsonString(string jsonstr)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<AgentInfo>(jsonstr);
        }

        public void Save(string path)
        {
            StreamWriter sw = new StreamWriter(path,false);
            sw.Write(getJsonString());
            sw.Close();
        }

        public static AgentInfo CreatFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return new AgentInfo();
            }
            StreamReader sr = new StreamReader(path);
            string s = sr.ReadToEnd();
            sr.Close();
            try 
            {
                return CreatFromJsonString(s);
            }
            catch
            {
                File.Delete(path);
                return new AgentInfo();
            }
        }
    }
}
