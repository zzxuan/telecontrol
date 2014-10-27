using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeleController
{
    public class TeleContans
    {
        public const string UdpIP = "230.0.0.136";
        public const int UdpPort = 54321;


        public const byte CmdMouse = 0xF0;
        public const byte CmdKey = 0xF1;
        public const byte CmdStart = 0xF2;
        public const byte CmdStop = 0xF3;
        
        public const byte MsgComputer = 0x00;
        public const byte MsgClientAsk = 0x01;
        public const byte MsgString = 0x02;
    }
}
