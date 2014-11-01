package com.telecontrol.activity;

public class TeleContans {
	public static final String UdpIP = "230.0.0.136";
	public static final int UdpPort = 54321;

	public static final byte CmdMouse = (byte) 0xF0;
	public static final byte CmdKey = (byte) 0xF1;
	public static final byte CmdStart = (byte) 0xF2;
	public static final byte CmdStop = (byte) 0xF3;

	public static final byte MsgComputer = 0x00;
	public static final byte MsgClientAsk = 0x01;
	public static final byte MsgString = 0x02;
}