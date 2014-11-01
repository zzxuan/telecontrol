package com.telecontrol.teleController;


import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;

public class ServerMsgInfo {
	public String getHostName() {
		return hostName;
	}
	public void setHostName(String hostName) {
		this.hostName = hostName;
	}
	public String getServerIP() {
		return serverIP;
	}
	public void setServerIP(String serverIP) {
		this.serverIP = serverIP;
	}
	public int getServerPort() {
		return serverPort;
	}
	public void setServerPort(int serverPort) {
		this.serverPort = serverPort;
	}
	String hostName;

	String serverIP;

	int serverPort;

	
	public ServerMsgInfo() {
		
	}
	public ServerMsgInfo(String js) {
		try {

    	JSONObject jo =JSON.parseObject(js);
    	hostName=jo.getString("HostName");
    	serverIP=jo.getString("ServerIP");
    	serverPort=jo.getIntValue("ServerPort");
		
	} catch (Exception e) {
		// TODO: handle exception
	}
	}
}