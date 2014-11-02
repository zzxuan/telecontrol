package com.socket.util;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.lang.reflect.InvocationTargetException;
import java.net.Socket;

public class TcpSocketClient {
	String host = "127.0.0.1";  //要连接的服务端IP地址  
    int port = 8899;   //要连接的服务端对应的监听端口  
	recivedataEvent rEvent;
	Socket client;
	
	public void connect() {
		new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				try {
					start();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}).start();
	}
	
	
	public void start() throws IOException {

	     //与服务端建立连接  
	     client = new Socket(host, port);  
	     new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				try {
					InputStream inputStream=client.getInputStream();
					recivedata(inputStream);
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					try {
						client.close();
					} catch (IOException e1) {
						// TODO Auto-generated catch block
						e1.printStackTrace();
					}  
				}
			}
		}).start();
	}
	
	public void close() {
		try {
			client.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public TcpSocketClient(String host, int port, recivedataEvent rEvent) {
		super();
		this.host = host;
		this.port = port;
		this.rEvent = rEvent;
	}




	public void send(final byte[] bytes) {
		new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				senddata(bytes);
			}
		}).start();
	}
	
	void senddata(byte[] bytes){
		try {
			int len = (bytes.length + 4);
			byte[] head=SocketUtil.intToByte(len);
			OutputStream outputStream=client.getOutputStream();
			outputStream.write(SocketUtil.arraycat(head, bytes));
			outputStream.flush();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	
	
	void recivedata(InputStream inputStream) throws IOException{
		byte[] bytes = new byte[1024*1024];
        byte[] buf = new byte[0];
        int len = 0;
        int num = 0;

        while (true)
        {
            int n = inputStream.read(bytes);
            if (n == 0)
                continue;

            byte[] b = new byte[n];
            System.arraycopy(bytes, 0, b, 0, n);
            buf=SocketUtil.arraycat(buf, b);
            num += n;
            if (num > 4 && len == 0)
            {
                len = SocketUtil.bytesToInt(buf);
            }
            if (len > 0 && buf.length >= len)
            {
                int m = 0;
                while (true)
                {
                    byte[] data = new byte[len];
                    System.arraycopy(buf, 4, data, 0, len-4);
                    
                    //--------------
                    if (rEvent != null)
                    {
                    	rEvent.recive(data);
                    }
                    //--------------
                    m += len;
                    if (buf.length - m < len)
                    {
                        byte[] ss = new byte[buf.length - m];
                        System.arraycopy(buf, m, ss, 0, buf.length - m);
                        buf = ss;
                        len = 0;
                        num = buf.length;
                        break;
                    }
                }
            }
        }
	}
	
	public interface recivedataEvent{
		public void recive(byte[] bytes) ;
	}
}
