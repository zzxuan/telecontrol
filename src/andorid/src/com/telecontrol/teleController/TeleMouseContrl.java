package com.telecontrol.teleController;

import com.socket.util.SocketUtil;

public class TeleMouseContrl {
	public class TeleMouseEventEnum {
		public final static int LeftDown = 0;
		public final static int LeftUp = 1;
		public final static int Move = 2;
		public final static int RightDown = 3;
		public final static int RightUp = 4;
	}

	public float get_X() {
		return _X;
	}

	public void set_X(float _X) {
		this._X = _X;
	}

	public float get_Y() {
		return _Y;
	}

	public void set_Y(float _Y) {
		this._Y = _Y;
	}

	public int get_TeleMouseEvent() {
		return _TeleMouseEvent;
	}

	public void set_TeleMouseEvent(int _TeleMouseEvent) {
		this._TeleMouseEvent = _TeleMouseEvent;
	}

	float _X;

	float _Y;

	int _TeleMouseEvent;

	public byte[] ToBytes() {
		byte[] bytes = new byte[13];
		bytes[0] = TeleContans.CmdMouse;
		byte[] bx = SocketUtil.float2byte(_X);
		byte[] by = SocketUtil.float2byte(_Y);
		byte[] be = SocketUtil.intToByte((int) _TeleMouseEvent);
		System.arraycopy(bx, 0, bytes, 1, 4);
		System.arraycopy(by, 0, bytes, 5, 4);
		System.arraycopy(be, 0, bytes, 9, 4);
		return bytes;
	}

//	public boolean FromBytes(byte[] bytes) {
//		if (bytes[0] != TeleContans.CmdMouse)
//			return false;
//		_X = SocketUtil..ToSingle(bytes, 1);
//		_Y = BitConverter.ToSingle(bytes, 5);
//		_TeleMouseEvent = (TeleMouseEventEnum) BitConverter.ToInt32(bytes, 9);
//		return true;
//	}

}