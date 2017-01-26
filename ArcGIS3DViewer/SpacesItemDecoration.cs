 
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;

namespace ArcGIS3DViewer
{
    public    class SpacesItemDecoration: RecyclerView.ItemDecoration
    {
        private int space;

        public SpacesItemDecoration(int space)
        {
            this.space = space;
        }

        public override void GetItemOffsets( Rect outRect, View view, RecyclerView parent, Android.Support.V7.Widget.RecyclerView.State state)
        {
            outRect.Left = space;
            outRect.Right = space;
            outRect.Bottom = space;
            if (parent.GetChildAdapterPosition(view) == 0)
            {
                outRect.Top = space;
            }
        }
    }
}