package com.telecontrol.activity;

import com.socket.util.TcpSocketClient;
import com.socket.util.TcpSocketClient.recivedataEvent;

import android.R.integer;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.view.SurfaceHolder;
import android.view.SurfaceHolder.Callback;
import android.view.SurfaceView;

public class TeleScrenSurfaceView extends SurfaceView implements Callback {

	private int screenW;
	private int screenH;
	private boolean flag;
	private Canvas canvas;
	private SurfaceHolder sfh;
	private String ip;
	private int port;

	public TeleScrenSurfaceView(Context context,String ip,int port) {
		super(context);
		// TODO Auto-generated constructor stub
		sfh = this.getHolder();
		sfh.addCallback(this);
		setFocusable(true);
		
		this.ip=ip;
		this.port=port;
		paint=new Paint();
	}

	@Override
	public void surfaceChanged(SurfaceHolder arg0, int arg1, int arg2, int arg3) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void surfaceCreated(SurfaceHolder arg0) {
		// TODO Auto-generated method stub
		screenW = this.getWidth();
		screenH = this.getHeight();
		flag = true;
		Startcontrl();
		
	}
	
	TcpSocketClient tcpSocketClient;
	private Paint paint;
	void Startcontrl(){
		tcpSocketClient=new TcpSocketClient(ip, port, new recivedataEvent() {
			
			@Override
			public void recive(byte[] bytes) {
				// TODO Auto-generated method stub
				draw(bytes);
			}
		});
		tcpSocketClient.connect();
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		tcpSocketClient.send(new byte[]{TeleContans.CmdStart});
	}
	
	
	void draw(byte[] bytes){
		try {
			canvas = sfh.lockCanvas();
			if (canvas != null) {
				Bitmap bitmap=BitmapFactory.decodeByteArray(bytes, 0, bytes.length);
				canvas.drawBitmap(bitmap, new Rect(0, 0, bitmap.getWidth(), bitmap.getHeight()),
				new Rect(0, 0, screenW, screenH),paint);
				bitmap.recycle();
			}
		} catch (Exception e) {
			// TODO: handle exception
		} finally {
			if (canvas != null)
				sfh.unlockCanvasAndPost(canvas);
		}
	}
	
	/**
	 * 绘制
	 */
	public void myDraw() {
		try {
			canvas = sfh.lockCanvas();
			if (canvas != null) {
				Paint paint= new Paint();
				paint.setColor(Color.RED);
				canvas.drawCircle(50, 50, 30, paint);
			}
		} catch (Exception e) {
			// TODO: handle exception
		} finally {
			if (canvas != null)
				sfh.unlockCanvasAndPost(canvas);
		}
	}

	@Override
	public void surfaceDestroyed(SurfaceHolder arg0) {
		// TODO Auto-generated method stub
		tcpSocketClient.close();
	}

}
