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
using Android.Content.PM;
using Android.Support.V7.Widget;
using Esri.ArcGISRuntime.Portal;
using System.IO;

namespace ArcGIS3DViewer
{
    [Activity(Label = "欢迎访问Portal WebScene", MainLauncher = false, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ProtalViewActivitycs : Activity
    {
        private List<Product> productList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProtalView);
            Button btnPortal = (Button)FindViewById(Resource.Id.btnPotal);
            btnPortal.Click += BtnPortal_Click;
        }

        private async void BtnPortal_Click(object sender, EventArgs e)
        {
            addProtal();
        }


        private async void addProtal()
        {
          
            ArcGISPortal portal = await ArcGISPortal.CreateAsync(new Uri("http://esrichina3d.arcgisonline.cn/arcgis/sharing/rest"));
            PortalQueryParameters para = PortalQueryParameters.CreateForItemsOfType(PortalItemType.WebScene);
            para.Limit = 20;
            PortalQueryResultSet<PortalItem> items = await portal.FindItemsAsync(para);
            int sum = 0;
            productList = new List<Product>();
            try
            {
                OpenFileActivity.item = items.Results.First();
                foreach (PortalItem item in items.Results)
                {
                    System.IO.Stream st = await item.GetDataAsync();
                    System.IO.StreamReader reader = new StreamReader(st);
                    st.Position = 0;
                    string json = reader.ReadToEnd();
                    reader.Close();
                    st.Close();
                    productList.Add(new Product(item.ThumbnailUri.ToString(), item.Title, json));
                }
            }
            catch
            { }

            RecyclerView recyclerView = (RecyclerView)FindViewById(Resource.Id.recyclerView);
            //设置layoutManager 布局模式
         //   recyclerView.SetLayoutManager(new StaggeredGridLayoutManager(3, StaggeredGridLayoutManager.Vertical));

            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            //设置adapter

            Context context = this.BaseContext;
            MasonryAdapter WebSceneItemsAdapter = new MasonryAdapter(productList, context);
            WebSceneItemsAdapter.ItemClick += WebSceneItemsAdapter_ItemClick;
            recyclerView.SetAdapter(WebSceneItemsAdapter);

            //设置item之间的间隔
            SpacesItemDecoration decoration = new SpacesItemDecoration(16);
            recyclerView.AddItemDecoration(decoration);
            
        }

        private void WebSceneItemsAdapter_ItemClick(object sender, int e)
        {
            try
            {
                string tag = productList[e].PortalItemJson;
                int pos = int.Parse(tag.Substring(0, 1));
                string json = tag.Substring(1);
                var SceneViewActivity = new Intent(this, typeof(MainActivity));
                Bundle bundle = new Bundle();
                bundle.PutString("PortalItemJson", json);
                bundle.PutString("name", productList[e].Title);
                SceneViewActivity.PutExtra("bundle", bundle);
                StartActivity(SceneViewActivity);
            }
            catch (Exception ex)
            { }
        }


    }
}