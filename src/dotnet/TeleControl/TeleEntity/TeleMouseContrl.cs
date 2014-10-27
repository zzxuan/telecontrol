﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;

namespace TeleController
{
    public enum TeleMouseEventEnum
    {
        LeftDown,
        LeftUp,
        Move,
        RightDown,
        RightUp
    }
    public class TeleMouseContrl:ITeleContrl
    {
        float _X;

        public float X
        {
            get { return _X; }
            set { _X = value; }
        }
        float _Y;

        public float Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        TeleMouseEventEnum _TeleMouseEvent;

        public TeleMouseEventEnum TeleMouseEvent
        {
            get { return _TeleMouseEvent; }
            set { _TeleMouseEvent = value; }
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[13];
            bytes[0] = TeleContans.CmdMouse;
            byte[] bx = BitConverter.GetBytes(_X);
            byte[] by = BitConverter.GetBytes(_Y);
            byte[] be = BitConverter.GetBytes((int)_TeleMouseEvent);
            Array.Copy(bx, 0, bytes, 1, 4);
            Array.Copy(by, 0, bytes, 5, 4);
            Array.Copy(be, 0, bytes, 9, 4);
            return bytes;
        }


        public bool FromBytes(byte[] bytes)
        {
            if (bytes[0] != TeleContans.CmdMouse)
                return false;
            _X = BitConverter.ToSingle(bytes, 1);
            _Y = BitConverter.ToSingle(bytes, 5);
            _TeleMouseEvent = (TeleMouseEventEnum)BitConverter.ToInt32(bytes, 9);
            return true;
        }

        public void SetMouseEvent()
        {
            double W = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double H = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度

            int x =(int)( W * _X);
            int y = (int)(H * _Y);

            switch (_TeleMouseEvent)
            {
                case TeleMouseEventEnum.LeftDown:
                    mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                    break;
                case TeleMouseEventEnum.LeftUp:
                    mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                    break;
                case TeleMouseEventEnum.Move:
                    SetCursorPos(x, y);
                    break;
                case TeleMouseEventEnum.RightDown:
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                    break;
                case TeleMouseEventEnum.RightUp:
                    mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
                    break;
            }
        }



        /// <summary>
        /// 鼠标控制参数
        /// </summary>
        const int MOUSEEVENTF_LEFTDOWN = 0x2;
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP = 0x40;
        const int MOUSEEVENTF_MOVE = 0x1;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        const int MOUSEEVENTF_RIGHTUP = 0x10;

        /// <summary>
        /// 鼠标的位置
        /// </summary>
        public struct PONITAPI
        {
            public int x, y;
        }

        [DllImport("user32.dll")]
        static extern int GetCursorPos(ref PONITAPI p);

        [DllImport("user32.dll")]
        static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    }
}
