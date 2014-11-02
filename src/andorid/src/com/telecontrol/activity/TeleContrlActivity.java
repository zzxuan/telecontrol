package com.telecontrol.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

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
		setContentView(new TeleScrenSurfaceView(this,ip,port));
	}
}
