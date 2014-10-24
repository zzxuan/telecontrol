using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeleController
{
    public interface ITeleContrl
    {
        /// <summary>
        /// 转为 bytes
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();
        /// <summary>
        /// 由 bytes构造
        /// </summary>
        /// <param name="bytes"></param>
        bool FromBytes(byte[] bytes);
    }
}
