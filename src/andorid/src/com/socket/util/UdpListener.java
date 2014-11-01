package com.socket.util;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;

import android.R.integer;

public class UdpListener {
	public UdpListener(int port, IreciveNotify notify) {
		super();
		this.port = port;
		this.notify = notify;
	}

	int port;
	Thread thread;
	IreciveNotify notify;
	DatagramSocket server;
	public void start() {
		thread = new Thread(new Runnable() {

			@Override
			public void run() {
				// TODO Auto-generated method stub
				try {
					server = new DatagramSocket(port);
					byte[] recvBuf = new byte[1024];
					DatagramPacket recvPacket = new DatagramPacket(recvBuf,
							recvBuf.length);
					while (true) {
						server.receive(recvPacket);
						if(notify!=null){
							notify.receiveBuf(recvPacket.getData(), recvPacket.getLength());
						}
					}

				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					if(server!=null)
						server.close();
				}
			}
		});
		thread.start();
	}
	
	public interface IreciveNotify{
		public void receiveBuf(byte[] bytes,int length);
	}
}
