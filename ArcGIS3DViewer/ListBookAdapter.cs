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
using UniversalImageLoader.Core;
using System.IO;
using Android.Graphics;
using Android.Util;

namespace ArcGIS3DViewer
{
   public  class ListBookAdapter : BaseAdapter
    {
        private List<string> Mproducts;
        private Context mContext;
        private LayoutInflater mInflater;
       
        public ListBookAdapter(List<string> products, Context context)
        {
            this.Mproducts = products;
            this.mContext = context;
            mInflater = LayoutInflater.From(context);
        }
        public override int Count
        {
            get
            {
               return this.Mproducts.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.Mproducts[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            if (convertView == null)
            {
              
                holder = new ViewHolder(this.mContext);

                convertView = mInflater.Inflate(Resource.Layout.bookMarkItem, null);

                holder.ImageView = (ImageView)convertView.FindViewById(Resource.Id.bookmarkImage);
                holder.TextView = (TextView)convertView.FindViewById(Resource.Id.bookmarktile);
                convertView.Tag=holder;
            }
            else
            {

                holder = (ViewHolder)convertView.Tag;
            }
         

            string base64=  Mproducts[position].ToString().Replace("data:image/jpeg;base64,", string.Empty);
            byte[] decode = Base64.Decode(base64, Base64Flags.Default);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(decode, 0, decode.Length);
            holder.ImageView.SetImageBitmap(bitmap);
            holder.TextView.Text = "ÄãºÃ";
            return convertView;
        }
    }

    public class ViewHolder: View
    {
        private ImageView imageView;
        private TextView textView;

        public ImageView ImageView
        {
            get
            {
                return imageView;
            }

            set
            {
                imageView = value;
            }
        }

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

        public ViewHolder(Context context):base(context)
        {
           
        }
    }
}