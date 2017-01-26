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
using Android.Support.V7.Widget;

namespace ArcGIS3DViewer
{
  public  class MasonryView: RecyclerView.ViewHolder
    {
      
        public ImageView imageView { get; private set; }
        public TextView textView { get; private set; }

        public MasonryView(View itemView, Action<int> listener) :base(itemView)
        {
            imageView = (ImageView)itemView.FindViewById(Resource.Id.masonry_item_img);
            textView = (TextView)itemView.FindViewById(Resource.Id.masonry_item_title);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}