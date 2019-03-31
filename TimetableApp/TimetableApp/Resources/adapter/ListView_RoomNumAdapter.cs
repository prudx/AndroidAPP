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
    class ListView_RoomNumAdapter : BaseAdapter
    {

        class ViewHolder : Java.Lang.Object
        {
            public TextView RoomNum { get; set; }
            public TextView IsBusy { get; set; }
        }

        private List<int> roomNums;
        private List<bool> isBusy;
        private Activity activity;
        private string busy;
        private string free;
        

        public ListView_RoomNumAdapter(Activity ac, List<int> rn, List<bool> ib, string b, string f)
        {
            this.activity = ac;
            this.roomNums = rn;
            this.isBusy = ib;
            this.busy = b;
            this.free = f;

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

            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListView_RoomNum, parent, false);

            var RoomNum = view.FindViewById<TextView>(Resource.Id.roomNum);

            var IsBusy = view.FindViewById<TextView>(Resource.Id.isBusy);

            RoomNum.Text = roomNums[position].ToString();


            //decide free or busy
            if (isBusy[position] == false)
            {
                IsBusy.Text = free;
                IsBusy.SetBackgroundColor(Color.LightGreen);
            }
            else
            {
                IsBusy.Text = busy;
                IsBusy.SetBackgroundColor(Color.MediumVioletRed);
            }



            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return roomNums.Count;
            }
        }

    }


}