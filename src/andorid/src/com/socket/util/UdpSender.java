package com.socket.util;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;

public class UdpSender {
	public static void send(final byte[] bytes,final String ip,final int port) {
		new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				DatagramSocket client;
				try {
					client = new DatagramSocket();
					InetAddress addr = InetAddress.getByName(ip);
					DatagramPacket sendPacket = new DatagramPacket(bytes,
							bytes.length, addr, port);
					client.send(sendPacket);
					client.close();
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}).start();
	}
}
