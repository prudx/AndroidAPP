using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EDMTDialog;
using Plugin.Connectivity;
using QuickType;
using Refit;
using TimetableApp.API;
using TimetableApp.Model;
using TimetableApp.Resources.adapter;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace TimetableApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public enum AlertType { Error, Load }
        ITimetableAPI timetableAPI;
        EditText editText;
        string queryString;
        int queryInt;
        ListView listTimetable;
        FloatingActionButton fab;
        AlertDialog dialog;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar 
                = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            listTimetable 
                = FindViewById<ListView>(Resource.Id.listView_Timetable);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            

            //update query string to the edit text box
            editText = FindViewById<EditText>(Resource.Id.queryInput);
            editText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                queryString = e.Text.ToString();

                try                     //trys to convert to int for later usage
                {
                    queryInt = Convert.ToInt32(queryString);
                }
                catch
                {
                    Toast.MakeText(this, "Please enter a number", ToastLength.Long).Show();
                }
            };

            //initiate API connection
            timetableAPI = RestService.For<ITimetableAPI>("https://timetablescheduler.azurewebsites.net");
        }

        //search button press
        private async void FabOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                CloseKeyboard();
                if (!CrossConnectivity.Current.IsConnected)     //connection check, to see if worth calling API
                {
                    CreateAlert(AlertType.Error, "No internet connection.");
                    return;
                }

                if (queryString == null)                        //type of search
                {
                    CreateAlert(AlertType.Load, "Searching for all rooms");                 
                }
                else
                {
                    CreateAlert(AlertType.Load, "Searching for room "+queryString);
                }

                //API SEARCH ALL
                List<Welcome> results = await timetableAPI.GetAllRooms();
                var roomOrder = results.OrderBy(c => c.StartTime);          //sort by start time
                results = roomOrder.ToList();

                //API SEARCH ROOM SPECIFIC
                if (queryString != null)            //wont be reached if blank query
                {
                    foreach (Welcome r in results)
                    {
                        var roomSpecific = results.Where(c => c.Room.RoomNo == queryInt);
                        results = roomSpecific.ToList();
                    }
                }
                

                //Console.WriteLine(results[0].Room.RoomNo);
                //Console.WriteLine(results[6].Room.RoomNo);


                var adapter = new ListView_TimetableAdapter(this, results);
                listTimetable.Adapter = adapter;
                

                if (dialog.IsShowing)
                    dialog.Dismiss();


            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }



            /*
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            */
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        //Alert handler to improve code cleanliness
        public AlertDialog CreateAlert(AlertType type, string alertMessage)
        {
            if(type == AlertType.Error)
            {
                AlertDialog.Builder dialogConnection = new AlertDialog.Builder(this);
                dialog = dialogConnection.Create();
                dialog.SetTitle("Connection Error");
                dialog.SetMessage(alertMessage);  
            }
            else if (type == AlertType.Load)
            {
                dialog = new EDMTDialogBuilder()
                    .SetContext(this)
                    .SetMessage(alertMessage)
                    .Build();
            }
            
            dialog.Show();

            return dialog;
        }

        //hide keyboard method (after pressing search for example)
        private void CloseKeyboard()
        {
            View view = this.CurrentFocus;
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
	}
}

/*  code hell
 
    //timetableAPI = RestService.For<ITimetableAPI>("https://jsonplaceholder.typicode.com");
    //timetableAPI = RestService.For<ITimetableAPI>("https://169.254.80.80:5001"); 
    //169.254.80.80 //192.168.56.1 //10.0.2.2 //192.168.0.123 // 192.168.0.123

     
     */
