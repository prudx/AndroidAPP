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
using TimetableApp.Resources.adapter;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace TimetableApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public enum AlertType { Error, Load, Info }
        ITimetableAPI timetableAPI;
        EditText editText;
        string queryString;
        int queryInt;
        ListView listTimetable;
        FloatingActionButton fab;
        AlertDialog dialog;
        string DayToSearch;
     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var infoMessage = Resources.GetText(Resource.String.infoMessage);
            var infoTitle = Resources.GetText(Resource.String.infoTitle);
            CreateAlert(AlertType.Info, infoMessage, infoTitle);

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

                if(queryString.Length == 0)     //reset to avoid triggering room not found alert later
                    queryString = null;

                try                     //trys to convert to int for later usage
                {
                    queryInt = Convert.ToInt32(queryString);
                }
                catch
                {
                    var inputError = Resources.GetText(Resource.String.inputError);
                    Toast.MakeText(this, inputError, ToastLength.Long).Show();
                }

            };

            //initiate API connection
            timetableAPI = RestService.For<ITimetableAPI>("https://timetablescheduler.azurewebsites.net");
            DayToSearch = DateTime.Now.DayOfWeek.ToString();  //set current day variable
            
        }




        //search button press
        private async void FabOnClick(object sender, EventArgs eventArgs)
        {
            //fetch localized strings (these get passed to adapters)
            var busy = Resources.GetText(Resource.String.busy);
            var free = Resources.GetText(Resource.String.free);

            try
            {
                CloseKeyboard();
                if (!CrossConnectivity.Current.IsConnected)     //connection check, to see if worth calling API
                {
                    var errorConnectionMessage = Resources.GetText(Resource.String.errorConnectionMessage);
                    var errorConnectionTitle = Resources.GetText(Resource.String.errorConnectionTitle);
                    CreateAlert(AlertType.Error, errorConnectionMessage, errorConnectionTitle);
                    return;
                }

                List<Welcome> results = new List<Welcome>();
                //API SEARCH ALL ROOM NUMBERS AND SHOW IF FREE NOW
                if (queryString == null)        
                {
                    var searchAll = Resources.GetText(Resource.String.searchAll);
                    CreateAlert(AlertType.Load, searchAll, ""); //type of search
                    results = await timetableAPI.GetAllRooms();
                    var roomOrder = results.OrderBy(c => c.Room.RoomNo);         //sort by room num using LINQ
                    results = roomOrder.ToList();

                    List<int> roomNum = new List<int>();           //used to further refine and add to below list
                    List<int> roomNumOrdered = new List<int>();
                    List<bool> isRoomBusy = new List<bool>();

                    //get unique room numbers
                    foreach (Welcome result in results)
                    {
                        if (!roomNum.Contains((int)result.Room.RoomNo))
                        {
                            roomNum.Add((int)result.Room.RoomNo);
                        }
                    }

                    //sort by room where has a timetable for right now
                    var orderByTimeNow = results
                                            .OrderBy(c => c.Room.RoomNo)
                                            .Where(c => c.StartTime.Day.ToString() == DayToSearch &&        
                                                        c.StartTime.Hour == DateTime.UtcNow.Hour);
                                        
                    results = orderByTimeNow.ToList();

                    //if true, labs have timetables scheduled for right now
                    if (results.Count != 0)
                    {
                        for (int i = 0; i < roomNum.Count; i++)
                        {
                            foreach (Welcome result in results)
                            {
                                if (result.Room.RoomNo == roomNum[i])
                                {
                                    //we add to the lists similtaniously to keep the pair in order
                                    roomNumOrdered.Add((int)result.Room.RoomNo);
                                    isRoomBusy.Add(result.Room.isBusy);             
                                }
                            }
                        }

                        for (int i = 0; i < roomNum.Count; i++)
                        {
                            //for rooms not already in the list
                            if (!roomNumOrdered.Contains(roomNum[i]))
                            {
                                roomNumOrdered.Add(roomNum[i]);
                                isRoomBusy.Add(false);              
                            }
                        }
                    }
                    //no timetable data == the room is definetly free
                    else
                    {
                        for (int i = 0; i < roomNum.Count; i++)
                        {
                            roomNumOrdered.Add(roomNum[i]);
                            isRoomBusy.Add(false);
                        }
                    }

                    //create list of room nums and their status + pass localized busy & free strings to adapter
                    
                    var adapterRoomNum = new ListView_RoomNumAdapter(this, roomNumOrdered, isRoomBusy, busy, free);
                    listTimetable.Adapter = adapterRoomNum;

                    if (dialog.IsShowing)
                        dialog.Dismiss();
                    return;
                }
                else if (queryString != null)   //GET specific room timetable for TODAY
                {
                    var searchSpecificRoom1 = Resources.GetText(Resource.String.searchSpecificRoom1);
                    var searchSpecificRoom2 = Resources.GetText(Resource.String.searchSpecificRoom2);
                    CreateAlert(AlertType.Load, searchSpecificRoom1 +" " +queryString +searchSpecificRoom2 +" " + DayToSearch, "");
                    results = await timetableAPI.GetRoomOnDay(queryInt, DayToSearch);             
                    var roomOrder = results.OrderBy(c => c.StartTime.Hour);      //sort by start time using LINQ 
                    results = roomOrder.ToList();

                    if (results.Count == 0)      //if room doesn't exist, display alert
                    {
                        dialog.Dismiss();
                        var errorRoomNotExistMessage = Resources.GetText(Resource.String.errorRoomNotExistMessage);
                        var errorRoomNotExistTitle = Resources.GetText(Resource.String.errorRoomNotExistTitle);
                        CreateAlert(AlertType.Error, errorRoomNotExistMessage+" "+DayToSearch, errorRoomNotExistTitle);
                        return;
                    }
                }
                
                var adapter = new ListView_TimetableAdapter(this, results, busy, free);
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
            if (id == Resource.Id.action_monday)
            {
                DayToSearch = DayOfWeek.Monday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_tuesday)
            {
                DayToSearch = DayOfWeek.Tuesday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_wednesday)
            {
                DayToSearch = DayOfWeek.Wednesday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_thursday)
            {
                DayToSearch = DayOfWeek.Thursday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_friday)
            {
                DayToSearch = DayOfWeek.Friday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_saturday)
            {
                DayToSearch = DayOfWeek.Saturday.ToString();
                return true;
            }
            else if (id == Resource.Id.action_sunday)
            {
                DayToSearch = DayOfWeek.Sunday.ToString();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        //Alert handler to improve code cleanliness
        public AlertDialog CreateAlert(AlertType type, string alertMessage, string alertTitle)
        {
            if(type == AlertType.Error)
            {
                AlertDialog.Builder dialogConnection = new AlertDialog.Builder(this);
                dialog = dialogConnection.Create();
                dialog.SetTitle(alertTitle);
                dialog.SetMessage(alertMessage);  
            }
            else if (type == AlertType.Load)
            {
                dialog = new EDMTDialogBuilder()
                    .SetContext(this)
                    .SetMessage(alertMessage)
                    .Build();
            }
            else if (type == AlertType.Info)
            {
                AlertDialog.Builder dialogConnection = new AlertDialog.Builder(this);
                var btnOk = Resources.GetText(Resource.String.btnOk);
                dialogConnection.SetPositiveButton(btnOk, (senderAlert, args) => {
                    dialogConnection.Dispose();
                });
                dialog = dialogConnection.Create();
                dialog.SetTitle(alertTitle);
                dialog.SetMessage(alertMessage);
                dialog.SetCanceledOnTouchOutside(true);
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
