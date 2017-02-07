using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Android.Widget;
using Java.IO;
using Esri.ArcGISRuntime.Portal;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Content;

namespace ArcGIS3DViewer
{
    [Activity (Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       
        
        Button btn;
        SceneView _sceneLeftView = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set the view from the "Main" layout resource
            SetContentView(Resource.Layout.Main);

            
            _sceneLeftView = FindViewById<SceneView>(Resource.Id.sceneView1);
            _sceneLeftView.Scene = new Scene();

            Intent intn = this.Intent;
            Bundle itemBundle= intn.GetBundleExtra("bundle");

            string json= itemBundle.GetString("PortalItemJson");
            this.Title = itemBundle.GetString("name");

            InitScene(json, _sceneLeftView);

            //var myLeftScene = new Scene(Basemap.CreateImageryWithLabels());
            //ArcGISSceneLayer jianzulayer = new ArcGISSceneLayer();
            //string jianzupath = Android.OS.Environment.ExternalStorageDirectory + "/ArcGIS/spk/niaocao.spk";
            //Uri jianzuLurl = new Uri(jianzupath);
            //jianzulayer.Source = jianzuLurl;
            //myLeftScene.OperationalLayers.Add(jianzulayer);

            //_sceneLeftView.Scene = myLeftScene;

            //_sceneLeftView.SetViewpointCameraAsync(new Esri.ArcGISRuntime.Mapping.Camera(39.9784413968177, 116.392566249097, 578.881929496303, 359.243366990266, 70.063780667993, 0), TimeSpan.FromSeconds(10));


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
                                    sceneView.Scene.OperationalLayers.Add(Scenelayer);
                                    //Label lab = new Label { Content = $"图层{LayerCount} ： {GrouplLayers_title}" };
                                    //this.listLayer.Items.Add(lab);
                                }
                            }
                        }
                        else
                        {
                            string operational_url = jo["operationalLayers"][j]["url"].ToString();
                            ArcGISSceneLayer Scenelayer = new ArcGISSceneLayer(new Uri(operational_url));
                            sceneView.Scene.OperationalLayers.Add(Scenelayer);
                            //Label lab = new Label { Content = $"图层{LayerCount} ： {operational_title}" };
                            //this.listLayer.Items.Add(lab);
                        }
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
                    for (int j = 0; j < jo["presentation"]["slides"].Count(); j++, bookMarkCount++)
                    {
                        string sttt = JsonConvert.SerializeObject(jo["presentation"]["slides"][j]["viewpoint"]);
                        string bkname = jo["presentation"]["slides"][j]["title"]["text"].ToString();
                        string bkimage = jo["presentation"]["slides"][j]["thumbnail"]["url"].ToString();
                        Viewpoint vp = Viewpoint.FromJson(sttt);
                        Bookmark bk = new Bookmark(bkname, vp);
                        sceneView.Scene.Bookmarks.Add(bk);

                        
                      //  Camera ca = new Camera();
                        //  Label lab = new Label { Content = $"书签{bookMarkCount} ： {bkname}" };
                        //lab.Tag = vp;
                        //lab.MouseDown += Lab_MouseDown;

                     
                        //  this.listbookMark.Items.Add(lab);
                        //double slidesx = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["x"];
                        //double slidesy = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["y"];
                        //double slidesz = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["position"]["z"];
                        //double slides_heading = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["heading"];
                        //double slides_tilt = (double)jo["presentation"]["slides"][j]["viewpoint"]["camera"]["tilt"];
                    }

                }

                string initialState = JsonConvert.SerializeObject(jo["initialState"]["viewpoint"]);
                Viewpoint initialViewpoint = Viewpoint.FromJson(initialState);
                await sceneView.SetViewpointAsync(initialViewpoint, TimeSpan.FromSeconds(10));
            }
            catch
            { }
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
        private async void Btn_Click(object sender, EventArgs e)
        {
            
        }
        private bool checkEndsWithInStringArray(String checkItsEnd,  String[] fileEndings)
        {
            foreach (String aEnd in fileEndings)
            {
                if (checkItsEnd.EndsWith(aEnd))
                    return true;
            }
            return false;
        }

         
    }
}