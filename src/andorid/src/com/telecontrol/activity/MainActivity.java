package com.telecontrol.activity;

import java.net.InetAddress;
import java.net.UnknownHostException;

import com.example.telecontrol.R;
import com.socket.util.SocketUtil;
import com.socket.util.TcpSocketClient;
import com.socket.util.TcpSocketClient.recivedataEvent;
import com.socket.util.UdpListener;
import com.socket.util.UdpListener.IreciveNotify;
import com.socket.util.UdpSender;
import com.tele.manger.ut.Jmanager;
import com.telecontrol.teleController.ServerMsgInfo;
import com.telecontrol.teleController.TeleContans;

import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.util.Log;
import android.util.Xml.Encoding;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;

public class MainActivity extends Activity {

	int clientPort = 35336;
	int _serverPort = 54322;
	SharedPreferences sharedPreferences;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		setTitle("XXF桌面");
		sharedPreferences=getSharedPreferences("xxftele", 0);
		init();
		udpInit();
		
	       Jmanager adManager = Jmanager.getInstance(this, 1, "26c603a4-9316-4c7c-a0db-28722202362b", 3);
	       //adManager.set(null, true, true);
	       adManager.start(); 
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		System.exit(0);
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		// TODO Auto-generated method stub
		if(keyCode == KeyEvent.KEYCODE_BACK){
			//System.exit(0);
			//return true;
		}
		return super.onKeyDown(keyCode, event);
	}
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(android.view.MenuItem item) {
		switch (item.getItemId()) {
		case R.id.action_refresh:
			listAdapter.clearItems();
			sendServerMsg();
			Toast.makeText(this, "刷新完成", Toast.LENGTH_SHORT).show();
			break;
		case R.id.action_about://关于
			Toast.makeText(this, "作者邮箱:xiaoxuanfengzzx@163.com", Toast.LENGTH_SHORT).show();
			break;
		default:
			break;
		}
		return super.onOptionsItemSelected(item);
	};

	ListView listView;
	MyListAdapter listAdapter;
	EditText edit;
	Button button;

	void init() {
		Log.i("", "telecc:" + "初始化");
		button = (Button) findViewById(R.id.button1);
		button.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				enterNext();
			}
		});
		edit = (EditText) findViewById(R.id.editText1);
		edit.setHint("请输入或选择IP");
		String ip=sharedPreferences.getString("edit","");
		edit.setText(ip);
		
		listView = (ListView) findViewById(R.id.listView1);
		if (listAdapter == null) {
			listAdapter = new MyListAdapter(this);
			listView.setAdapter(listAdapter);
			Log.i("", "telecc:" + "setAdapter");
		}

		listView.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View arg1, int arg2,
					long arg3) {
				// TODO Auto-generated method stub
				ServerMsgInfo msgInfo = (ServerMsgInfo) listAdapter
						.getObject(arg2);
				edit.setText(msgInfo.getServerIP());
				_serverPort = msgInfo.getServerPort();
			}
		});

		// 隐藏小键盘
		getWindow().setSoftInputMode(
				WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
	}

	/**
	 * 进入下一个页面
	 */
	void enterNext() {
		String ip = edit.getText().toString();
		
		try {
			InetAddress addr = InetAddress.getByName(ip);
			Intent intent = new Intent(this, TeleContrlActivity.class);
			intent.putExtra("ip", ip);
			intent.putExtra("port", _serverPort);
			Editor edit=sharedPreferences.edit();
			edit.putString("edit", ip);
			edit.commit();
			startActivity(intent);
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();

		}
	}

	void udpInit() {
		UdpListener listener = new UdpListener(clientPort, new IreciveNotify() {

			@Override
			public void receiveBuf(byte[] bytes, int length) {
				// TODO Auto-generated method stub
				final byte[] buf = new byte[length - 1];
				System.arraycopy(bytes, 1, buf, 0, buf.length);
				switch (bytes[0]) {
				case TeleContans.MsgString:
					MainActivity.this.runOnUiThread(new Runnable() {
						public void run() {
							String string = new String(buf);
							ServerMsgInfo msgInfo = new ServerMsgInfo(string);
							Toast.makeText(MainActivity.this,
									msgInfo.getHostName(), Toast.LENGTH_SHORT)
									.show();
							listAdapter.addItem(msgInfo.getHostName(),
									msgInfo.getServerIP(), msgInfo);
						}
					});
					break;
				}
			}
		});
		listener.start();
		sendServerMsg();
	}

	void sendServerMsg() {
		byte[] bb = SocketUtil.intToByte(clientPort);
		byte[] b1 = new byte[] { TeleContans.MsgClientAsk };
		UdpSender.send(SocketUtil.arraycat(b1, bb), TeleContans.UdpIP,
				TeleContans.UdpPort);
	}

}
