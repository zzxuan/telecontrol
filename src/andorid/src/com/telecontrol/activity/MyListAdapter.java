package com.telecontrol.activity;
import java.util.ArrayList;    
import java.util.HashMap;    
import java.util.List;    
import java.util.Map;    

import com.example.telecontrol.R;
    
import android.R.integer;
import android.content.Context;    
import android.util.Log;
import android.view.LayoutInflater;    
import android.view.View;    
import android.view.ViewGroup;    
import android.widget.BaseAdapter;    
import android.widget.CheckBox;    
import android.widget.ImageView;    
import android.widget.TextView;    
    
public class MyListAdapter extends BaseAdapter {    
    private LayoutInflater mInflater;    
    //private List<Map<String, Object>> mData;    
    private ArrayList<MidData> midDatas=new ArrayList<MyListAdapter.MidData>();
    
    public MyListAdapter(Context context) {    
        mInflater = LayoutInflater.from(context);    
        init();    
    }    
    
    public void addItem(String title,String info,Object obj) {
        MidData midData=new MidData();
        midData.titleString=title;
        midData.info=info;
        midData.object=obj;
        midDatas.add(midData);
        notifyDataSetChanged();
        //Log.i("list", "telecc:"+title+","+midDatas);
	}
    
    public Object getObject(int n) {
		if(midDatas.size()>n)
		{
			return midDatas.get(n).object;
		}
		return null;
	}
    
    public void clearItems() {
    	midDatas.clear();
    	//Log.i("list", "telecc: 清理"+midDatas.size());
    	notifyDataSetChanged();
	}
    
    //初始化    
    private void init() {    
        
    }    
    
    @Override    
    public int getCount() {    
    	//Log.i("list", "telecc:Count="+midDatas.size()+":"+midDatas);
        return midDatas.size();    
    }    
    
    @Override    
    public Object getItem(int position) {    
        return null;    
    }    
    
    @Override    
    public long getItemId(int position) {    
        return 0;    
    }    
    
    @Override    
    public View getView(int position, View convertView, ViewGroup parent) {    
        ViewHolder holder = null;    
        //convertView为null的时候初始化convertView。    
        if (convertView == null) {    
            holder = new ViewHolder();    
            convertView = mInflater.inflate(R.layout.comlistitem, null);    
            holder.title = (TextView) convertView.findViewById(R.id.titletext);    
            holder.info=(TextView) convertView.findViewById(R.id.infotext);
            convertView.setTag(holder);    
        } else {    
            holder = (ViewHolder) convertView.getTag();    
        }    
        holder.title.setText(midDatas.get(position).titleString.toString());    
        holder.info.setText(midDatas.get(position).info.toString());
        return convertView;    
    }    
    
    public final class ViewHolder {    
        public TextView title;    
        public TextView info; 
    }    
    
    public class MidData {
    	public String titleString;
    	public String info;
    	public Object object;
    	
    	@Override
    	public String toString() {
    		// TODO Auto-generated method stub
    		return "["+titleString+"]";
    	}
	}
    
}  