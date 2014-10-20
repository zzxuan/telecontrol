using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using ServerDemo;

namespace ServerDemo.ServerHelp
{
    public class Server//启动服务
    {
        /// <summary>
        /// 基于WCF服务的类型（typeof(IDataBaseService)）
        /// 创建了ServieHost对象，并添加了一个终结点。
        /// 具体的地址为http://localhost:9999/ServerDemo，
        /// 采用了WSHttpBinding，并指定了服务契约的类型ICalculator。
        /// </summary>
        public static void start()
        {
            WebServiceHost host = new WebServiceHost(typeof(DataService));
            {


                host.AddServiceEndpoint(typeof(IDataBaseService), new WebHttpBinding(), "http://localhost:9999/ServerDemo");
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    //添加元数据发布，主要用于给客户端提示 可以不要
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri("http://localhost:9999/ServerDemo/metadata");
                    host.Description.Behaviors.Add(behavior);
                    //**********************************end
                }
                host.Opened += delegate
               {
                   System.Windows.MessageBox.Show("服务已启动");
               };

                host.Open();
                Console.Read();
            }
        }
    }
}
