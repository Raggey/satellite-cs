using System;


namespace Satellite_cs{


  public class Transform{

    public double longitude;
    public double latitude;
    public double height;

    private double pi;
    private double twoPi;
    private double rad2deg;
    private double deg2rad;


    public Transform() {

      Globals globals = new Globals();
      pi = globals.pi;
      twoPi = globals.twoPi;
      rad2deg = globals.rad2deg;
      deg2rad = globals.deg2rad;
    }

    public double radiansToDegrees(double radians) {
      return radians * rad2deg;
    }

    public double degreesToRadians(double degrees) {
      return degrees * deg2rad;
    }

    public double degreesLat(double radians) {
      if (radians < (-pi / 2) || radians > (pi / 2)) {
        throw new ArgumentOutOfRangeException("Latitude radians must be in range [-pi/2; pi/2].");
      }
      return radiansToDegrees(radians);
    }

    public double degreesLong(double radians) {
      if (radians < -pi || radians > pi) {
        throw new ArgumentOutOfRangeException("Longitude radians must be in range [-pi; pi].");
      }
      return radiansToDegrees(radians);
    }

    public double radiansLat(double degrees) {
      if (degrees < -90 || degrees > 90) {
        throw new ArgumentOutOfRangeException("Latitude degrees must be in range [-90; 90].");
      }
      return degreesToRadians(degrees);
    }

    public double radiansLong(double degrees) {
      if (degrees < -180 || degrees > 180) {
        throw new ArgumentOutOfRangeException("Longitude degrees must be in range [-180; 180].");
      }
      return degreesToRadians(degrees);
    }


    public Ecf geodeticToEcf(ObserverGd geodetic) {

      double longitude = geodetic.longitude;
      double latitude = geodetic.latitude;
      double height = geodetic.height;

      double a = 6378.137;
      double b = 6356.7523142;
      double f = (a - b) / a;
      double e2 = ((2 * f) - (f * f));
      double normal = a / Math.Sqrt(1 - (e2 * (Math.Sin(latitude) * Math.Sin(latitude))));

      double x = (normal + height) * Math.Cos(latitude) * Math.Cos(longitude);
      double y = (normal + height) * Math.Cos(latitude) * Math.Sin(longitude);
      double z = ((normal * (1 - e2)) + height) * Math.Sin(latitude);

      Ecf ecf = new Ecf();
      ecf.x = x;
      ecf.y = y;
      ecf.z = z;
      
      return ecf;
    }

    // public Geodetic eciToGeodetic(Eci eci, Gmst gmst) {
    //   // http://www.celestrak.com/columns/v02n03/
    //   double a = 6378.137;
    //   double b = 6356.7523142;
    //   double R = Math.Sqrt((eci.x * eci.x) + (eci.y * eci.y));
    //   double f = (a - b) / a;
    //   double e2 = ((2 * f) - (f * f));

    //   double longitude = Math.Atan2(eci.y, eci.x) - gmst;
    //   while (longitude < -pi) {
    //     longitude += twoPi;
    //   }
    //   while (longitude > pi) {
    //     longitude -= twoPi;
    //   }

    //   double kmax = 20;
    //   double k = 0;
    //   double latitude = Math.Atan2(
    //     eci.z,
    //     Math.Sqrt((eci.x * eci.x) + (eci.y * eci.y)),
    //   );
    //   double C;
    //   while (k < kmax) {
    //     C = 1 / Math.Sqrt(1 - (e2 * (Math.Sin(latitude) * Math.Sin(latitude))));
    //     latitude = Math.Atan2(eci.z + (a * C * e2 * Math.Sin(latitude)), R);
    //     k += 1;
    //   }
    //   double height = (R / Math.Cos(latitude)) - (a * C);


    //   return { longitude, latitude, height };
    // }














  }

}