

using System;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Util;
using Object = Java.Lang.Object;

namespace TestingServiceWitheGPS.Service
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }

    public class MyServiceConnection: Object, IServiceConnection
    {
        public bool isConnected { get; private set; }
        public MyServiceBinder mBinder { get; private set; }
        private static readonly string TAG = typeof(MyServiceConnection).FullName;

        private MyActivity myActivity;

        public MyServiceConnection(MyActivity my)
        {
            isConnected = false;
            mBinder = null;
            myActivity = my;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            // cast the binder located by the OS as our local binder subclass
            if (service is MyServiceBinder serviceBinder)
            {
                mBinder = serviceBinder;
                mBinder.IsBound = true;
                Log.Debug("ServiceConnection", "OnServiceConnected Called");
                // raise the service connected event
                ServiceConnected(this, new ServiceConnectedEventArgs {Binder = service});

                // now that the Service is bound, we can start gathering some location data
                serviceBinder.myService.StartLocationUpdates();
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            mBinder = null;
            mBinder.IsBound = false;
            
        }

        public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };
    }

}