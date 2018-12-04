

using Android.Locations;

namespace TestingServiceWitheGPS
{
    public class MyLocation : IMyLocation
    {

        public MyLocation()
        {
            Longitude = 0;
            Latitude = 0;

        }

        MyLocation(double lo, double la)
        {
            Longitude = lo;
            Latitude = la;
        }

        public MyLocation GetLocation()
        {

            return this;
        }
    }
}