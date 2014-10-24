using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeleController
{
    public class TeleContans
    {
        public const byte CmdMouse = 0xF0;
        public const byte CmdKey = 0xF1;
        
        public const byte MsgComputer = 0x00;
        public const byte MsgClientAsk = 0x01;
    }
}
