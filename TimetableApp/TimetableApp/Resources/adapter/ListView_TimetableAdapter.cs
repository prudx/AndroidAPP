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
using QuickType;
using TimetableApp.Model;

namespace TimetableApp.Resources.adapter
{
    class ListView_TimetableAdapter : BaseAdapter
    {

        class ViewHolder : Java.Lang.Object
        {
            public TextView Time { get; set; }
            public TextView BusyStatus { get; set; }
        }

        private Activity activity;
        List<Welcome> timetable;

        public ListView_TimetableAdapter(Activity ac, List<Welcome> t)
        {
            this.activity = ac;
            this.timetable = t;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListView_Timetable, parent, false);

            var Time = view.FindViewById<TextView>(Resource.Id.time);
            var BusyStatus = view.FindViewById<TextView>(Resource.Id.busyStatus);

            Time.Text = timetable[position].StartTime.Hour.ToString();
            BusyStatus.Text = timetable[position].Room.Description;

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return timetable.Count;
            }
        }

    }

    
}