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
using Esri.ArcGISRuntime.Portal;

namespace ArcGIS3DViewer
{
    [Activity(Label = "OpenFileActivity")]
    public class OpenFileActivity : Activity
    {

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

      public  static PortalItem item = null;
    }
}