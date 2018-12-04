using Android.OS;

namespace TestingServiceWitheGPS.Service
{
    public class MyServiceBinder :Binder
    {
        public bool IsBound { get; set; }

        public MyService myService;

        public MyServiceBinder(MyService Service)
        {
            this.myService = Service;
        }

        public MyService MyService => myService;

        //do to
        //add GetMethode ... 

    }

}