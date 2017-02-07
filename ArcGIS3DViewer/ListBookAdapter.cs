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
using Esri.ArcGISRuntime.Mapping;

namespace ArcGIS3DViewer
{
    public class ListBookAdapter : BaseAdapter
    {
        List<ItemBookMark> itemBookMarks = null;
        private Context mContext;
        private LayoutInflater mInflater;

        public ListBookAdapter(List<ItemBookMark> bks, Context context)
        {
            this.itemBookMarks = bks;
            this.mContext = context;
            mInflater = LayoutInflater.From(context);
        }
        public override int Count
        {
            get
            {
                return this.itemBookMarks.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.itemBookMarks[position];
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
                convertView.Tag = holder;
            }
            else
            {

                holder = (ViewHolder)convertView.Tag;
            }
            string base64 = itemBookMarks[position].BookMarkImage.Replace("data:image/jpeg;base64,", string.Empty);
            byte[] decode = Base64.Decode(base64, Base64Flags.Default);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(decode, 0, decode.Length);
            holder.ImageView.SetImageBitmap(bitmap);
            holder.TextView.Text = itemBookMarks[position].BookMarkName;
            return convertView;
        }
    }

    public class ViewHolder : View
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

        public ViewHolder(Context context) : base(context)
        {

        }
    }

    public class ItemBookMark: Java.Lang.Object
    {
        private string bookMarkName = string.Empty;
        private string bookMarkImage = string.Empty;
        private Bookmark portalBookmark = null;

        public ItemBookMark(string name, string strImage, Bookmark bk)
        {
            this.bookMarkName = name;
            this.bookMarkImage = strImage;
            this.portalBookmark = bk;
        }

        public string BookMarkName
        {
            get
            {
                return bookMarkName;
            }

            set
            {
                bookMarkName = value;
            }
        }

        public string BookMarkImage
        {
            get
            {
                return bookMarkImage;
            }

            set
            {
                bookMarkImage = value;
            }
        }

        public Bookmark PortalBookmark
        {
            get
            {
                return portalBookmark;
            }

            set
            {
                portalBookmark = value;
            }
        }
    }
}