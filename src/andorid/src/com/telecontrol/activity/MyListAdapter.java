package com.telecontrol.activity;
import java.util.ArrayList;    
import java.util.HashMap;    
import java.util.List;    
import java.util.Map;    

import com.example.telecontrol.R;
    
import android.R.integer;
import android.content.Context;    
import android.view.LayoutInflater;    
import android.view.View;    
import android.view.ViewGroup;    
import android.widget.BaseAdapter;    
import android.widget.CheckBox;    
import android.widget.ImageView;    
import android.widget.TextView;    
    
public class MyListAdapter extends BaseAdapter {    
    private LayoutInflater mInflater;    
    private List<Map<String, Object>> mData;    
    
    public MyListAdapter(Context context) {    
        mInflater = LayoutInflater.from(context);    
        init();    
    }    
    
    public void addItem(String title,String info,Object obj) {
    	Map<String, Object> map = new HashMap<String, Object>();    
        map.put("title", title);    
        map.put("info", info); 
        map.put("obj", obj);
        mData.add(map);
        notifyDataSetChanged();
	}
    
    public Object getObject(int n) {
		if(mData.size()>n)
		{
			return mData.get(n).get("obj");
		}
		return null;
	}
    
    public void clearItems() {
    	mData.clear();
    	notifyDataSetChanged();
	}
    
    //初始化    
    private void init() {    
        mData=new ArrayList<Map<String, Object>>();    
    }    
    
    @Override    
    public int getCount() {    
        return mData.size();    
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
        holder.title.setText(mData.get(position).get("title").toString());    
        holder.info.setText(mData.get(position).get("info").toString());
        return convertView;    
    }    
    
    public final class ViewHolder {    
        public TextView title;    
        public TextView info; 
    }    
}  