using Android.App;
using Android.Content;
using Android.OS;
using TestingServiceWitheGPS.Service;
using Android.Support.V4.Content;

namespace TestingServiceWitheGPS
{
    public class MyActivity : Activity
    {
        public MyServiceConnection myServiceConnection { get; private set; }


        public void BindServiceOnStart()
        {
            if (myServiceConnection == null)
            {
                myServiceConnection = new MyServiceConnection(this);
            }

            // Example of creating an explicit Intent in an Android Activity
            BindService();
        }

        public void BindService()
        {
            //Intent serviceToStart = new Intent(this, typeof(MyService));
            Intent serviceToStart = new Intent(Application.Context, typeof(MyService));

            // Check if device is running Android 8.0 or higher and if so, use the newer StartForegroundService() method
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Application.Context.StartForegroundService(new Intent(Application.Context, typeof(MyService)));
            }
            else // For older versions, use the traditional StartService() method
            {
                Application.Context.StartService(new Intent(Application.Context, typeof(MyService)));
            }

            Application.Context.BindService(serviceToStart, myServiceConnection, Bind.AutoCreate);
        }

        public void StopService()
        {
            // Unbind from the LocationService; otherwise, StopSelf (below) will not work:
            if (myServiceConnection != null)
            {
                UnbindService(myServiceConnection);
            }
        }
    }
}