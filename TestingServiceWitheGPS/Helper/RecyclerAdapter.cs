using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ListPopupWindow = Android.Support.V7.Widget.ListPopupWindow;

namespace TestingServiceWitheGPS.Helper
{
    public class RecyclerHolder : RecyclerView.ViewHolder
    {
        public TextView Longitude {get; set; }
        public TextView Latitude  {get; set; }
        public TextView TimeStamp {get; set; }

        public RecyclerHolder(View itemView):base(itemView)
        {
            Latitude = itemView.FindViewById<TextView>(Resource.Id.Latitude);
            Longitude = itemView.FindViewById<TextView>(Resource.Id.Longitude);
            TimeStamp = itemView.FindViewById<TextView>(Resource.Id.TimeStamp);
        }
    }

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private readonly List<RecyclerItem> _recyclerItems;

        public override int ItemCount => _recyclerItems.Count;

        public RecyclerAdapter(List<RecyclerItem> recyclerItems)
        {
            _recyclerItems = recyclerItems ?? throw new ArgumentNullException(nameof(recyclerItems));
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is RecyclerHolder viewHolder)
            {
                viewHolder.Latitude.Text  = "Latitude  : " +  _recyclerItems[position].Latitude.ToString();
                viewHolder.Longitude.Text = "Longitude : " +  _recyclerItems[position].Longitude.ToString();
                viewHolder.TimeStamp.Text = "TimeStamp : " +_recyclerItems[position].TimeStamp.ToString("HH:mm:ss") + "Uhr";
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.recycler_item,parent,false);

            return new RecyclerHolder(itemView);
        }
    }
}