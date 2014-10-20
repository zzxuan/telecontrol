using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TeleServer.WcfServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class TeleService : ITeleService
    {
        public string GetData()
        {
            return string.Format("You entered: {0}", 123);
        }

    }
}
