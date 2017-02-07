using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ArcGIS3DViewer
{
   public class ListLayerAdapter : BaseAdapter
    {
        List<ItemLayer> itemLayers = null;
        private Context mContext;
        private LayoutInflater mInflater;

        public ListLayerAdapter(List<ItemLayer> layers, Context context)
        {
            this.itemLayers = layers;
            this.mContext = context;
            mInflater = LayoutInflater.From(context);
        }
        public override int Count
        {
            get
            {
                return this.itemLayers.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.itemLayers[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayerViewHolder holder = null;
            if (convertView == null)
            {
                holder = new LayerViewHolder(this.mContext);
                convertView = mInflater.Inflate(Resource.Layout.LayerItem, null);
                holder.TextView = (TextView)convertView.FindViewById(Resource.Id.LayerItemName);
                convertView.Tag = holder;
            }
            else
            {

                holder = (LayerViewHolder)convertView.Tag;
            }
            holder.TextView.Text = itemLayers[position].LayerName;
            return convertView;
        }
    }

    public class LayerViewHolder : View
    {
        
        private TextView textView;

        
        public TextView TextView
        {
            get
            {
                return textView;
            }

            set
            {
                textView = value;
            }
        }

        public LayerViewHolder(Context context) : base(context)
        {

        }
    }


    public class ItemLayer : Java.Lang.Object
    {
        private string layerName = string.Empty;
         
      

        public ItemLayer(string name)
        {
            this.layerName = name;
             
        }

        public string LayerName
        {
            get
            {
                return layerName;
            }

            set
            {
                layerName = value;
            }
        }
    }

}