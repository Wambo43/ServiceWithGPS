using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestingServiceWitheGPS.Helper;
using System.Timers;

namespace TestingServiceWitheGPS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        //public MyServiceConnection myServiceConnection;
        static readonly string             TAG = "MainActivity";
        public          MyActivity         myActivity;
        static readonly int                RC_REQUEST_LOCATION_PERMISSION = 1000;
        static readonly string[]           REQUIRED_PERMISSIONS           = {Manifest.Permission.AccessFineLocation};
        public          List<RecyclerItem> myLocations                    = new List<RecyclerItem>();

        private Button                     startServieButton;
        private RecyclerView               recyclerView;
        private RecyclerAdapter            recyclerAdapter;
        private RecyclerView.LayoutManager layoutManager;
        private Timer                      _timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //This code into OnCreate method
            if (myActivity == null)
            {
                myActivity = new MyActivity();
            }

            layoutManager   = new LinearLayoutManager(this);
            recyclerAdapter = new RecyclerAdapter(myLocations);

            startServieButton = FindViewById<Button>(Resource.Id.start_servive_button);
            recyclerView      = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(recyclerAdapter);
            startServieButton.Click += StartServiceByButton;


            for (int i = 0; i < 7; ++i)
            {
                myLocations.Add(new RecyclerItem() {Longitude = 01.0d, Latitude = 01.0d, TimeStamp = DateTime.Now});
                RunOnUiThread(() =>
                {
                    Log.Debug(TAG, "Huhu vom UI Thread");
                    recyclerAdapter.NotifyDataSetChanged();
                    recyclerView.SmoothScrollToPosition(myLocations.Count - 1);
                });
            }

            _timer = new Timer();
            _timer.Interval = 10000;
            _timer.Enabled = true;

            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            AddToRecycler();
        }

        protected override void OnStart()
        {
            base.OnStart();
            // Start the location service:
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int) Permission.Granted)
            {
                Log.Debug(TAG, "User already has granted permission.");
                myActivity.BindServiceOnStart();
            }
            else
            {
                Log.Debug(TAG, "Have to request permission from the user. ");
                RequestLocationPermission();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            //   myActivity.StopService();
        }

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "OnDestroy: Location app is becoming inactive");
            base.OnDestroy();

            // Stop the location service:
            myActivity.StopService();
        }


        void StartServiceByButton(object sender, System.EventArgs e)
        {
            AddToRecycler();
        }

        private void AddToRecycler()
        {
            Log.Debug(TAG, "All 10sec excecute");
            MyLocation a = myActivity.myServiceConnection.mBinder.myService.GetMyLocation();
            myLocations.Add(new RecyclerItem() {Longitude = a.Longitude, Latitude = a.Latitude, TimeStamp = DateTime.Now});
            RunOnUiThread( () => 
            {
                Log.Debug(TAG, "Huhu vom UI Thread");
                recyclerAdapter.NotifyDataSetChanged();
                recyclerView.SmoothScrollToPosition(myLocations.Count -1);
            });

            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RC_REQUEST_LOCATION_PERMISSION)
            {
                if (grantResults.Length == 1 &&
                    grantResults[0]     == Permission.Granted)
                {
                    Log.Debug(TAG, "User granted permission for location.");
                    myActivity.BindServiceOnStart();
                }
                else
                {
                    Log.Warn(TAG, "User did not grant permission for the location.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        void RequestLocationPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                var layout = FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(layout, Resource.String.permission_location_rationale, Snackbar.LengthIndefinite)
                        .SetAction("Ok",
                                   new Action<View>(delegate { ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS, RC_REQUEST_LOCATION_PERMISSION); }))
                        .Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS, RC_REQUEST_LOCATION_PERMISSION);
            }
        }
    }
}
