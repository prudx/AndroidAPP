using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QuickType;


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
        private List<Welcome> timetable;
        private string busy;
        private string free;

        public ListView_TimetableAdapter(Activity ac, List<Welcome> t, string b, string f)
        {
            this.activity = ac;
            this.timetable = t;
            busy = b;
            free = f;
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

            
            if (timetable[position].StartTime.Hour == 12)
            {   //pm
                Time.Text = timetable[position].StartTime.Hour.ToString() + "pm";
            }
            else if (timetable[position].StartTime.Hour > 12)
            {   //pm
                int time = (int)timetable[position].StartTime.Hour - 12;
                Time.Text = time.ToString() + "pm";
            }
            else
            {   //am
                Time.Text = timetable[position].StartTime.Hour.ToString() + "am";
            }

            //decide free or busy
            if(timetable[position].Room.isBusy == true)
            {
                BusyStatus.Text = busy;
                BusyStatus.SetBackgroundColor(Color.MediumVioletRed);

            }
            else
            {
                BusyStatus.Text = free;
                BusyStatus.SetBackgroundColor(Color.LightGreen);

            }

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