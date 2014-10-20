using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using System.ServiceModel.Web;

namespace ServerDemo.ServerHelp
{
    /// <summary>
    /// 定义接口
    /// wcf中服务端和客户端都有同样的接口
    /// 服务端有实现接口的函数，客户端没有
    /// 这样 客户端调用 服务端实现 有点类似与远程方法调用
    /// </summary>
    [ServiceContract]
    public interface IDataBaseService
    {

        #region 

        [WebGet(UriTemplate = "GetMsg/{msg}")]
        string GetMsg(string msg);
        #endregion

    }
}
