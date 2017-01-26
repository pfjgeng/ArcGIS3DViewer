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

namespace ArcGIS3DViewer
{
   public class Product:Java.Lang.Object
    {
        private string imgurl;
        private String title;
        private string portalItemJson;


        public Product(string image, String itle,string itemjson)
        {
            imgurl = image;
            title = itle;
            portalItemJson = itemjson;
        }

        public string Img
        {
            get
            {
                return imgurl;
            }

            set
            {
                imgurl = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string PortalItemJson
        {
            get
            {
                return portalItemJson;
            }

            set
            {
                portalItemJson = value;
            }
        }
    }
}