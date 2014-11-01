package com.telecontrol.activity;

import com.example.telecontrol.R;
import com.socket.util.SocketUtil;
import com.socket.util.UdpListener;
import com.socket.util.UdpListener.IreciveNotify;
import com.socket.util.UdpSender;
import com.telecontrol.teleController.ServerMsgInfo;

import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.util.Xml.Encoding;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;

public class MainActivity extends Activity {

	int clientPort = 35336;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		init();
		udpInit();
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

		default:
			break;
		}
		return super.onOptionsItemSelected(item);
	};

	ListView listView;
	MyListAdapter listAdapter;
	EditText edit;

	void init() {
		edit = (EditText) findViewById(R.id.editText1);
		listAdapter = new MyListAdapter(this);
		listView = (ListView) findViewById(R.id.listView1);
		listView.setAdapter(listAdapter);
		listView.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> arg0, View arg1, int arg2,
					long arg3) {
				// TODO Auto-generated method stub
				ServerMsgInfo msgInfo=(ServerMsgInfo)listAdapter.getObject(arg2);
				edit.setText(msgInfo.getServerIP());
			}
		});
		
		// 隐藏小键盘
		getWindow().setSoftInputMode(
				WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
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
							listAdapter.addItem(msgInfo.getHostName(),
									msgInfo.getServerIP(),msgInfo);
						}
					});
					break;
				}
			}
		});
		listener.start();
		sendServerMsg();
	}
	
	void sendServerMsg(){
		byte[] bb = SocketUtil.intToByte(clientPort);
		byte[] b1 = new byte[] { TeleContans.MsgClientAsk };
		UdpSender.send(SocketUtil.arraycat(b1, bb), TeleContans.UdpIP,
				TeleContans.UdpPort);
	}

}
