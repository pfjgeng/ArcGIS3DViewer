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
using Esri.ArcGISRuntime.UI.Controls;
using Android.Content.PM;
using Esri.ArcGISRuntime.Mapping;

namespace ArcGIS3DViewer
{
    [Activity(Label = "三维场景", MainLauncher = false, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SceneViewActivity : Activity
    {
        private SceneView _sceneLeftView;
        private ListView listLayer;
        private ListView listBookMark;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SceneViewLayout);
            _sceneLeftView = FindViewById<SceneView>(Resource.Id.SceneView);

            listLayer = FindViewById<ListView>(Resource.Id.listLayer);
            listBookMark = FindViewById<ListView>(Resource.Id.listbookmark);
            //Intent intn = this.Intent;
            //Bundle itemBundle = intn.GetBundleExtra("bundle");
            //string json = itemBundle.GetString("PortalItemJson");
            //this.Title = itemBundle.GetString("name");
            try
            {
                Context context = this.BaseContext;
                ListBookAdapter adapter = new ListBookAdapter(getData(), context);
                listBookMark.Adapter = adapter;
            }
            catch
            {
            }
          

            listBookMark.ItemLongClick += ListBookMark_ItemLongClick;


        }

        private void ListBookMark_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var myLeftScene = new Scene(Basemap.CreateImageryWithLabels());
            ArcGISSceneLayer jianzulayer = new ArcGISSceneLayer();
            string jianzupath = Android.OS.Environment.ExternalStorageDirectory + "/ArcGIS/spk/niaocao.spk";
            Uri jianzuLurl = new Uri(jianzupath);
            jianzulayer.Source = jianzuLurl;
            myLeftScene.OperationalLayers.Add(jianzulayer);

            _sceneLeftView.Scene = myLeftScene;

            _sceneLeftView.SetViewpointCameraAsync(new Esri.ArcGISRuntime.Mapping.Camera(39.9784413968177, 116.392566249097, 578.881929496303, 359.243366990266, 70.063780667993, 0), TimeSpan.FromSeconds(10));
        }

       

        private List<string> getData()
        {
            List<String> data = new List<String>();
            data.Add("测试数据1");
            data.Add("测试数据2");
            data.Add("测试数据3");
            data.Add("测试数据4");
            return data;
        }
    }
}