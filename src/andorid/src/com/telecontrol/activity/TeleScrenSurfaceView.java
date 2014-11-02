package com.telecontrol.activity;

import com.socket.util.TcpSocketClient;
import com.socket.util.TcpSocketClient.recivedataEvent;
import com.telecontrol.teleController.TeleContans;
import com.telecontrol.teleController.TeleMouseContrl;
import com.telecontrol.teleController.TeleMouseContrl.TeleMouseEventEnum;

import android.R.integer;
import android.content.Context;
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

public class TeleScrenSurfaceView extends SurfaceView implements Callback,
		OnGestureListener {

	private int screenW;
	private int screenH;
	private boolean flag;
	private Canvas canvas;
	private SurfaceHolder sfh;
	private String ip;
	private int port;
	private GestureDetector gd;
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

		gd = new GestureDetector(this); // 创建手势监听对象
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

	@Override
	public boolean onTouchEvent(MotionEvent event) {
		// TODO Auto-generated method stub
		gd.onTouchEvent(event); // 通知手势识别方法
		x = event.getX();
		y = event.getY();
		switch (event.getAction()) {
		case MotionEvent.ACTION_DOWN:
			mevent = TeleMouseEventEnum.LeftDown;
			sendcmd();
			break;
		case MotionEvent.ACTION_MOVE:
			mevent = TeleMouseEventEnum.Move;
			sendcmd();
			break;
		case MotionEvent.ACTION_UP:
			mevent = TeleMouseEventEnum.LeftUp;
			sendcmd();
			break;
		default:
			break;
		}

		return true;
		// return super.onTouchEvent(event);
	}

	void longclick() {
		Log.i("", "longclick");
//		mevent = TeleMouseEventEnum.RightDown;
//		sendcmd();
		mevent = TeleMouseEventEnum.RightUp;
		sendcmd();
	}

	float x;
	float y;
	int mevent;

	void sendcmd() {
		//tcpSocketClient.send(getBytes(TeleMouseEventEnum.Move));
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

	// **************************下面是手势识别的重写方法*******************************************

	// 屏幕点下
	private String TAG = "GameView";

	public boolean onDown(MotionEvent arg0) {
		return false;
	}

	// 屏幕点下

	public boolean onFling(MotionEvent e1, MotionEvent e2, float velocityX,

	float velocityY) {
		return false;
	}

	// 屏幕点下 并长按时触发

	public void onLongPress(MotionEvent e) {

		Log.d(TAG, "onLongPress");
		longclick();
	}

	// 屏幕拖动

	public boolean onScroll(MotionEvent e1, MotionEvent e2, float distanceX,

	float distanceY) {
		return false;
	}

	// 屏幕长按

	public void onShowPress(MotionEvent e) {

		// TODO Auto-generated method stub
	}

	// 屏幕点击后弹起

	public boolean onSingleTapUp(MotionEvent e) {
		return false;

	}
}
