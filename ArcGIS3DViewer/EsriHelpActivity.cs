
using Android.App;
using Android.OS;
using Android.Webkit;

namespace ArcGIS3DViewer
{
    [Activity(Label = "EsriHelpActivity", MainLauncher = false)]
    public class EsriHelpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EsriHelp);
            WebView web_view = FindViewById<WebView>(Resource.Id.webview);
             web_view.Settings.JavaScriptEnabled = true;
             web_view.LoadUrl("https://developers.arcgis.com/arcgis-runtime/");

         

        }
    }

    public class HelloWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
}