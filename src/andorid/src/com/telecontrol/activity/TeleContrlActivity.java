package com.telecontrol.activity;

import android.app.Activity;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.view.Window;
import android.view.WindowManager;

public class TeleContrlActivity extends Activity {
	private String ip;
	private int port;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		Intent intent = getIntent();
		ip = intent.getStringExtra("ip");
		port = intent.getIntExtra("port", 54322);

		requestWindowFeature(Window.FEATURE_NO_TITLE);// 隐藏标题
		this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);// 去掉信息栏
		if (getRequestedOrientation() != ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) {
			setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);// 横屏
		}
		setContentView(new TeleScrenSurfaceView(this, ip, port));
	}
}
