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
using Java.Net;
using Android.Graphics;
using Esri.ArcGISRuntime.Portal;
using System.IO;
using UniversalImageLoader.Core;

namespace ArcGIS3DViewer
{
    public class MasonryAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        private  List<Product> Mproducts;
        private Context mContext;
        private UniversalImageLoader.Core.ImageLoader imageLoader;
        public MasonryAdapter(List<Product> products, Context context)
        {
            Mproducts = products;
            this.mContext = context;
            imageLoader = ImageLoader.Instance;
            imageLoader.Init(ImageLoaderConfiguration.CreateDefault(this.mContext));
            DisplayImageOptions displayImageOptions = new DisplayImageOptions.Builder()
                                         .ShowImageForEmptyUri(Resource.Drawable.image034)//ø’URLœ‘ æÕº∆¨
                                         .ShowImageOnFail(Resource.Drawable.image034)//º”‘ÿ ß∞‹œ‘ æÕº∆¨
                                         .ShowImageOnLoading(Resource.Drawable.image034)//’˝‘⁄º”‘ÿœ‘ æÕº∆¨
                                         .CacheInMemory(true)//ª∫¥ÊµΩƒ⁄¥Ê
                                         .CacheOnDisk(true)//ª∫¥ÊµΩSDø®
                                         .ResetViewBeforeLoading()
                                         .Build();

        }
        public override int ItemCount
        {
            get
            {
                return Mproducts.Count;
            }
        }
      
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
           
            MasonryView masonryView = holder as MasonryView;
            DisplayImageOptions displayImageOptions = new DisplayImageOptions.Builder()
                                     .ShowImageForEmptyUri(Resource.Drawable.image034)//ø’URLœ‘ æÕº∆¨
                                     .ShowImageOnFail(Resource.Drawable.image034)//º”‘ÿ ß∞‹œ‘ æÕº∆¨
                                     .ShowImageOnLoading(Resource.Drawable.image034)//’˝‘⁄º”‘ÿœ‘ æÕº∆¨
                                     .CacheInMemory(true)//ª∫¥ÊµΩƒ⁄¥Ê
                                     .CacheOnDisk(true)//ª∫¥ÊµΩSDø®
                                     .ResetViewBeforeLoading()
                                     .Build();
            imageLoader.DisplayImage(Mproducts[position].Img, masonryView.imageView, displayImageOptions);
           // masonryView.imageView.Click += ImageView_Click;
            masonryView.imageView.Tag =position.ToString()+ Mproducts[position].PortalItemJson;
            masonryView.textView.Text = Mproducts[position].Title;
        }
        //private async void ImageView_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string tag = ((ImageView)sender).Tag.ToString();
        //        int pos=int.Parse( tag.Substring(0, 1));
        //        string json=  tag.Substring(1);
        //        var SceneViewActivity = new Intent(this.mContext, typeof(MainActivity));
               
        //        Bundle bundle = new Bundle();
        //        bundle.PutString("PortalItemJson", json);
        //        bundle.PutString("name", Mproducts[pos].Title);
        //        SceneViewActivity.PutExtra("bundle", bundle);
        //        SceneViewActivity.AddFlags(ActivityFlags.NewTask);

        //        ProtalViewActivitycs act =    this.mContext as ProtalViewActivitycs;
        //        this.mContext.StartActivity(SceneViewActivity);
        //    }
        //    catch(Exception ex)
        //    { }
        //}
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.masonry_item, parent, false);

           
            return new MasonryView(view, OnClick);
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}