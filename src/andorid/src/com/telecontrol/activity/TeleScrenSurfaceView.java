package com.telecontrol.activity;

import com.socket.util.TcpSocketClient;
import com.socket.util.TcpSocketClient.ConnectState;
import com.socket.util.TcpSocketClient.recivedataEvent;
import com.telecontrol.teleController.TeleContans;
import com.telecontrol.teleController.TeleMouseContrl;
import com.telecontrol.teleController.TeleMouseContrl.TeleMouseEventEnum;

import android.R.integer;
import android.content.Context;
import android.content.pm.ActivityInfo;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.Log;
import android.view.GestureDetector;
import android.view.GestureDetector.OnGestureListener;
import android.view.MotionEvent;
import android.view.SurfaceHolder;
import android.view.View;
import android.view.SurfaceHolder.Callback;
import android.view.SurfaceView;
import android.widget.Toast;

public class TeleScrenSurfaceView extends SurfaceView implements Callback {

	private int screenW;
	private int screenH;
	private boolean flag;
	private Canvas canvas;
	private SurfaceHolder sfh;
	private String ip;
	private int port;

	private Context context;

	public TeleScrenSurfaceView(Context context, String ip, int port) {
		super(context);
		// TODO Auto-generated constructor stub
		this.context = context;
		sfh = this.getHolder();
		sfh.addCallback(this);
		setFocusable(true);

		this.ip = ip;
		this.port = port;
		paint = new Paint();

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

	void Startcontrl() {
		tcpSocketClient = new TcpSocketClient(ip, port, new recivedataEvent() {

			@Override
			public void recive(byte[] bytes) {
				// TODO Auto-generated method stub
				draw(bytes);
			}
		}, new ConnectState() {

			@Override
			public void notifyConnet(final int statetype) {
				// TODO Auto-generated method stub
				((TeleContrlActivity) context).runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						if (statetype == 1) {
							Toast.makeText(context, "连接失败", Toast.LENGTH_SHORT)
									.show();
							((TeleContrlActivity) context).finish();
						} else if (statetype == 2) {
							Toast.makeText(context, "连接中断", Toast.LENGTH_SHORT)
									.show();
						} else if (statetype == 3) {
							Toast.makeText(context, "发送失败", Toast.LENGTH_SHORT)
									.show();
						}

					}
				});

			}
		});
		tcpSocketClient.connect();
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		tcpSocketClient.send(new byte[] { TeleContans.CmdStart });
	}

	void draw(byte[] bytes) {
		try {
			canvas = sfh.lockCanvas();
			if (canvas != null) {
				Bitmap bitmap = BitmapFactory.decodeByteArray(bytes, 0,
						bytes.length);
				canvas.drawBitmap(bitmap, new Rect(0, 0, bitmap.getWidth(),
						bitmap.getHeight()), new Rect(0, 0, screenW, screenH),
						paint);
				bitmap.recycle();
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

	MotionEvent downEvent;
	boolean longclickflag=false;
	float dx,dy;
	@Override
	public boolean onTouchEvent(MotionEvent event) {
		// TODO Auto-generated method stub
		//Log.i("", "click" + event.toString());
		x = event.getX();
		y = event.getY();
		Log.i("", "click***** x:"+x+" y :"+y );
		switch (event.getAction()) {
		case MotionEvent.ACTION_DOWN:
			Log.i("", "click*****DOWN");
			sendcmd(TeleMouseEventEnum.LeftDown);
			downEvent = event;
			longclickflag=false;
			dx=x;
			dy=y;
			
			new Thread(new Runnable() {
				
				@Override
				public void run() {
					// TODO Auto-generated method stub
					try {
						Thread.sleep(1000);
						if(!longclickflag){
							longclick();
							longclickflag=true;
						}
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}).start();
			
			break;
		case MotionEvent.ACTION_MOVE:
			Log.i("1234", "click***** ssdx:"+Math.abs(x - dx) +" dasy :"+Math.abs(y - dy) );
			if (Math.abs(x - dx) < 1
					&& Math.abs(y - dy) < 1) {
				if(event.getEventTime()-event.getDownTime()>1000&&!longclickflag)
				{
					longclick();
					longclickflag=true;
				}
			}
			else {
				Log.i("", "click*****Move");
				sendcmd(TeleMouseEventEnum.Move);
				longclickflag=true;
			}

			break;
		case MotionEvent.ACTION_UP:
			sendcmd(TeleMouseEventEnum.LeftUp);
			longclickflag=true;
			Log.i("", "click*****up");
			break;
		default:
			break;
		}

		return true;
		// return super.onTouchEvent(event);
	}

	void longclick() {
		Log.i("", "click*****long");
		// mevent = TeleMouseEventEnum.RightDown;
		// sendcmd();
		//mevent = TeleMouseEventEnum.RightUp;
		sendcmd(TeleMouseEventEnum.RightUp);
	}

	float x;
	float y;
	

	void sendcmd(int mevent) {
		// tcpSocketClient.send(getBytes(TeleMouseEventEnum.Move));
		Log.i("", "click*****"+mevent);
		tcpSocketClient.send(getBytes(mevent));
	}

	byte[] getBytes(int event) {
		float xx = x / (float) screenW;
		float yy = y / (float) screenH;
		TeleMouseContrl tMouseContrl = new TeleMouseContrl();
		tMouseContrl.set_X(xx);
		tMouseContrl.set_Y(yy);
		tMouseContrl.set_TeleMouseEvent(event);
		return tMouseContrl.ToBytes();
	}

}
