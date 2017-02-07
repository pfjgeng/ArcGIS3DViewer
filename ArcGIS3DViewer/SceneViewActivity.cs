using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Esri.ArcGISRuntime.UI.Controls;
using Android.Content.PM;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Esri.ArcGISRuntime.Geometry;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.UI;
 

namespace ArcGIS3DViewer
{
    [Activity(Label = "三维场景", MainLauncher = false, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SceneViewActivity : Activity
    {
        private SceneView _sceneLeftView;
        private ListView listLayer;
        private ListView listBookMark;
        List<ItemBookMark> bks = new List<ItemBookMark>();
        List<ItemLayer> layers = new List<ItemLayer>();
        ArcGISSceneLayer jianzulayer = null;
        bool isRotate = false;
        Button btnSun;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SceneViewLayout);
            _sceneLeftView = FindViewById<SceneView>(Resource.Id.SceneView);
            _sceneLeftView.Scene = new Scene();
            _sceneLeftView.GeoViewTapped += _sceneLeftView_GeoViewTapped;

            listLayer = FindViewById<ListView>(Resource.Id.listLayer);
            listBookMark = FindViewById<ListView>(Resource.Id.listbookmark);
            listBookMark.ItemClick += ListBookMark_ItemClick;
            Button btnSPK = FindViewById<Button>(Resource.Id.btnspk);
            btnSPK.Click += BtnSPK_Click;

            Button btnRcenter = FindViewById<Button>(Resource.Id.button2);
            btnRcenter.Click += BtnRcenter_Click;

             btnSun = FindViewById<Button>(Resource.Id.button3);
            btnSun.Click += BtnSun_Click;

            Intent intn = this.Intent;
            Bundle itemBundle = intn.GetBundleExtra("bundle");
            string json = itemBundle.GetString("PortalItemJson");
            this.Title = itemBundle.GetString("name");
            InitScene(json, _sceneLeftView);
        }

        private async void BtnSun_Click(object sender, EventArgs e)
        {
            this._sceneLeftView.SunLighting = LightingMode.LightAndShadows;
            this._sceneLeftView.AtmosphereEffect = AtmosphereEffect.HorizonOnly;
            this._sceneLeftView.IsAttributionTextVisible = true;
            //this._sceneLeftView.AmbientLightColor = Color.Red;

            await SetSunView();
        }

        public async Task<bool> SetSunView()
        {
            for (int i = 0; i < 24; i++)
            {
                await Task.Delay(500);
                this._sceneLeftView.SunTime = DateTimeOffset.Now.LocalDateTime.AddHours(i);
                btnSun.Text = "日照：" + this._sceneLeftView.SunTime.LocalDateTime.Hour.ToString() + "时";
            }
            btnSun.Text = "日照";
            return true;
        }

        private void _sceneLeftView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            isRotate = false;
        }

        private async void BtnRcenter_Click(object sender, EventArgs e)
        {
            try
            {
                var screenCenter = new Android.Graphics.PointF(this._sceneLeftView.Width / 2, this._sceneLeftView.Height / 2);
                MapPoint ptCenter = await this._sceneLeftView.ScreenToLocationAsync(screenCenter);
                if (ptCenter == null) return;

                if (isRotate)
                {
                    isRotate = false;
                }
                else
                {
                    isRotate = true;
                }
                while (isRotate)
                {
                    await this._sceneLeftView.SetViewpointCameraAsync(this._sceneLeftView.Camera.RotateAround(ptCenter, 10, 0, 0));
                };

            }
            catch
            {
            }

        }

        private void BtnSPK_Click(object sender, EventArgs e)
        {
            if (jianzulayer == null)
            {
                jianzulayer = new ArcGISSceneLayer();
                string jianzupath = Android.OS.Environment.ExternalStorageDirectory + "/ArcGIS/spk/niaocao.spk";
                Uri jianzuLurl = new Uri(jianzupath);
                jianzulayer.Source = jianzuLurl;
                _sceneLeftView.Scene.OperationalLayers.Add(jianzulayer);
            }
            _sceneLeftView.SetViewpointCameraAsync(new Esri.ArcGISRuntime.Mapping.Camera(39.9784413968177, 116.392566249097, 578.881929496303, 359.243366990266, 70.063780667993, 0), TimeSpan.FromSeconds(6));
        }

        private void ListBookMark_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Camera cm = bks[e.Position].PortalBookmark.Viewpoint.Camera;
            _sceneLeftView.SetViewpointCameraAsync(cm);
        }

        

        public async void InitScene(string json, SceneView sceneView)
        {
            try
            {
                JsonReader jreader = new JsonTextReader(new System.IO.StringReader(json));
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                // 加载operationalLayers 图层
                if (jo["operationalLayers"].Count() > 0)
                {
                    int LayerCount = 1;
                    layers.Clear();
                    for (int j = 0; j < jo["operationalLayers"].Count(); j++, LayerCount++)
                    {
                        string operational_title = jo["operationalLayers"][j]["title"].ToString();
                        string operational_layerType = jo["operationalLayers"][j]["layerType"].ToString();
                        if (operational_layerType.Equals("GroupLayer"))
                        {
                            if (jo["operationalLayers"][j]["layers"] != null && jo["operationalLayers"][j]["layers"].Count() > 0)
                            {
                                for (int i = 0; i < jo["operationalLayers"][j]["layers"].Count(); i++)
                                {
                                    string GrouplLayers_title = jo["operationalLayers"][j]["layers"][i]["title"].ToString();
                                    string GrouplLayers_url = jo["operationalLayers"][j]["layers"][i]["url"].ToString();
                                    ArcGISSceneLayer Scenelayer = new ArcGISSceneLayer(new Uri(GrouplLayers_url));
                                   string Content = $"图层{LayerCount} ： {GrouplLayers_title}";
                                    sceneView.Scene.OperationalLayers.Add(Scenelayer);
                                    layers.Add(new ItemLayer(Content));
                                    //Label lab = new Label { Content = $"图层{LayerCount} ： {GrouplLayers_title}" };
                                }
                            }
                        }
                        else
                        {
                            string operational_url = jo["operationalLayers"][j]["url"].ToString();
                            ArcGISSceneLayer Scenelayer = new ArcGISSceneLayer(new Uri(operational_url));
                            sceneView.Scene.OperationalLayers.Add(Scenelayer);
                            string Content = $"图层{LayerCount} ： {operational_title}";
                            layers.Add(new ItemLayer(Content) );
                            //Label lab = new Label { Content = $"图层{LayerCount} ： {operational_title}" };
                         
                        }
                    }
                    if (layers.Count > 0)
                    {
                        Context context = this.BaseContext;
                        ListLayerAdapter adapter = new ListLayerAdapter(layers, context);
                        listLayer.Adapter = adapter;
                    }
                    
                }
                if (jo["baseMap"]["baseMapLayers"] != null && jo["baseMap"]["baseMapLayers"].Count() > 0)
                {
                    // 加载底图 baseMap
                    string baseMapLayers_layerType = jo["baseMap"]["baseMapLayers"][0]["layerType"].ToString();
                    if (baseMapLayers_layerType.Equals("ArcGISTiledMapServiceLayer"))
                    {
                        string baseMapLayers_url = jo["baseMap"]["baseMapLayers"][0]["url"].ToString();
                        ArcGISMapImageLayer basemap = new ArcGISMapImageLayer(new Uri(baseMapLayers_url));
                        Basemap map = new Basemap(basemap);
                        sceneView.Scene.Basemap = map;
                    }
                }
                // 加载高程 elevationLayers
                if (jo["baseMap"]["elevationLayers"] != null && jo["baseMap"]["elevationLayers"].Count() > 0)
                {
                    for (int j = 0; j < jo["baseMap"]["elevationLayers"].Count(); j++)
                    {
                        string elevationLayer_url = jo["baseMap"]["elevationLayers"][j]["url"].ToString();
                        string elevationLayer_layerType = jo["baseMap"]["elevationLayers"][j]["layerType"].ToString();
                        var elevationSource = new ArcGISTiledElevationSource(new System.Uri(elevationLayer_url));
                        sceneView.Scene.BaseSurface.ElevationSources.Add(elevationSource);
                    }
                }
                // 加载书签 slides
                if (jo["presentation"]["slides"] != null && jo["presentation"]["slides"].Count() >= 0)
                {
                    int bookMarkCount = 1;
                    bks.Clear();
                    for (int j = 0; j < jo["presentation"]["slides"].Count(); j++, bookMarkCount++)
                    {
                        string sttt = JsonConvert.SerializeObject(jo["presentation"]["slides"][j]["viewpoint"]);
                        string bkname = jo["presentation"]["slides"][j]["title"]["text"].ToString();
                        string bkimage = jo["presentation"]["slides"][j]["thumbnail"]["url"].ToString();

                        double targetGeometry_x = (double)jo["presentation"]["slides"][j]["viewpoint"]["targetGeometry"] ["x"];
                        double targetGeometry_y = (double)jo["presentation"]["slides"][j]["viewpoint"]["targetGeometry"] ["y"];
                        double targetGeometry_z = (double)jo["presentation"]["slides"][j]["viewpoint"]["targetGeometry"] ["z"];



                        double slidesx = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["x"];
                        double slidesy = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["y"];
                        double slidesz = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["z"];
                        double slides_heading = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["heading"];
                        double slides_tilt = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["tilt"];
                        int SRID = (int)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["spatialReference"]["latestWkid"];

                        
                        MapPoint cmpt = new MapPoint(slidesx, slidesy, slidesz, SpatialReference.Create(SRID));
                        Camera camera = new Camera(cmpt, slides_heading, slides_tilt,0);
                        MapPoint mp = new MapPoint(targetGeometry_x, targetGeometry_y, targetGeometry_z, SpatialReference.Create(SRID));

                        Viewpoint vp = new Viewpoint(mp,camera);
                        Bookmark bk = new Bookmark(bkname, vp);
                        sceneView.Scene.Bookmarks.Add(bk);
                        ItemBookMark itembk = new ItemBookMark(bkname, bkimage, bk);
                        bks.Add(itembk);
                    }
                    if (bks.Count > 0)
                    {
                        Context context = this.BaseContext;
                        ListBookAdapter adapter = new ListBookAdapter(bks, context);
                        listBookMark.Adapter = adapter;
                    }

                }

                double initCamera_x = (double)jo["initialState"]["viewpoint"]["camera"]["position"]["x"];
                double initCamera_y = (double)jo["initialState"]["viewpoint"]["camera"]["position"]["y"];
                double initCamera_z = (double)jo["initialState"]["viewpoint"]["camera"]["position"]["z"];
                double initCamera_heading = (double)jo["initialState"]["viewpoint"]["camera"]["heading"];
                double initCamera_tilt = (double)jo["initialState"]["viewpoint"]["camera"]["tilt"];
                int initSRID = (int)jo["initialState"]["viewpoint"]["camera"]["position"]["spatialReference"]["latestWkid"];
                MapPoint initcmpt = new MapPoint(initCamera_x, initCamera_y, initCamera_z, SpatialReference.Create(initSRID));
                 Camera initCamera = new Camera(initcmpt, initCamera_heading, initCamera_tilt, 0);
                 await _sceneLeftView.SetViewpointCameraAsync(initCamera, TimeSpan.FromSeconds(10));
            }
            catch(Exception ex)
            {

            }
        }
        //创建web Scene
        public async void InitScene(PortalItem webSceneItem, SceneView sceneView)
        {
            try
            {
                System.IO.Stream st = await webSceneItem.GetDataAsync();
                System.IO.StreamReader reader = new StreamReader(st);
                st.Position = 0;
                string strRes = reader.ReadToEnd();
                reader.Close();
                st.Close();
                InitScene(strRes, sceneView);
            }
            catch
            { }
        }



        
    }
}