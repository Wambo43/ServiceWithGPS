using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;

namespace TestingServiceWitheGPS.Service
{
    [Service(Name = "com.xamarin.TestingServieWitheGPS")]
    public class MyService : Android.App.Service, ILocationListener
    {
        public IBinder Binder { get; private set; }

        private MyLocation _myLocation;

        private readonly string _logTag = "MyServive located";

        protected LocationManager LocMgr = Application.Context.GetSystemService("location") as LocationManager;

        // This is any integer value unique to the application.
        private const int SERVICE_NOTIFICATION_ID = 10000;

        public override IBinder OnBind(Intent intent)
        {
            Binder = new MyServiceBinder(this);

            return Binder;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug("MyService", " is OnCreate");
            _myLocation = new MyLocation();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(_logTag, "LocationService started");

            var notification = new NotificationCompat.Builder(this).SetContentTitle(Resources.GetString(Resource.String.app_name))
                                                                   .SetContentText(Resources.GetString(Resource.String.notification_text))
                                                                   .SetSmallIcon(Resource.Drawable.abc_list_selector_holo_dark)
                                                                    //.SetContentIntent(BuildIntentToShowMainActivity())
                                                                   .SetOngoing(true)
                                                                    //.AddAction(BuildRestartTimerAction())
                                                                    //.AddAction(BuildStopServiceAction())
                                                                   .Build();

            // Enlist this instance of the service as a foreground service
            StartForeground(SERVICE_NOTIFICATION_ID, notification);

            return StartCommandResult.Sticky;
        }

        public void EndingCommand()
        {
            StopForeground(true);
        }

        //here the ILocationListener Methoden

        //Called when the location has changed.
        public void OnLocationChanged(Location location)
        {
            // This should be updating every time we request new location updates
            // both when teh app is in the background, and in the foreground
            Log.Debug(_logTag, $"Its called OnLocationChanged");
            Log.Debug(_logTag, $"Latitude is {location.Latitude}");
            Log.Debug(_logTag, $"Longitude is {location.Longitude}");
            Log.Debug(_logTag, $"Altitude is {location.Altitude}");
            Log.Debug(_logTag, $"Speed is {location.Speed}");
            Log.Debug(_logTag, $"Accuracy is {location.Accuracy}");
            Log.Debug(_logTag, $"Bearing is {location.Bearing}");

            _myLocation.Longitude = location.Longitude;
            _myLocation.Latitude  = location.Latitude;
        }

        public void StartLocationUpdates()
        {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy         = Accuracy.NoRequirement;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
            Log.Debug("", $"You are about to get location updates via {locationProvider}");


            // Get an initial fix on location
            LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);

            Log.Debug("", "Now sending location updates");
        }

        public MyLocation GetMyLocation()
        {
            return _myLocation;
        }

        //Called when the provider is disabled by the user.
        public void OnProviderDisabled(string provider)
        {
           // throw new System.NotImplementedException();
        }

        //Called when the provider is enabled by the user.
        public void OnProviderEnabled(string provider)
        {
            //throw new System.NotImplementedException();
        }

        //Called when the provider status changes.
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
           //throw new System.NotImplementedException();
        }
    }
}
