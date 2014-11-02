package com.telecontrol.activity;

import android.app.Activity;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.view.Window;

public class TeleContrlActivity extends Activity {
	private String ip;
	private int port;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		Intent intent=getIntent();
		ip=intent.getStringExtra("ip");
		port=intent.getIntExtra("port", 54322);
		requestWindowFeature(Window.FEATURE_NO_TITLE);//隐藏标题
		 if(getRequestedOrientation()!=ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE){
			  setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
			 }
		setContentView(new TeleScrenSurfaceView(this,ip,port));
	}
}
