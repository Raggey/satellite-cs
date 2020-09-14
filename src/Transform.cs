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


    public Coordinates geodeticToEcf(Geodetic geodetic) {

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

      Coordinates ecf = new Coordinates();
      ecf.x = x;
      ecf.y = y;
      ecf.z = z;
      
      return ecf;
    }

    public Geodetic eciToGeodetic(Coordinates eci, double gmst) {
      // http://www.celestrak.com/columns/v02n03/
      double a = 6378.137;
      double b = 6356.7523142;
      double R = Math.Sqrt((eci.x * eci.x) + (eci.y * eci.y));
      double f = (a - b) / a;
      double e2 = ((2 * f) - (f * f));

      double longitude = Math.Atan2(eci.y, eci.x) - gmst;
      while (longitude < -pi) {
        longitude += twoPi;
      }
      while (longitude > pi) {
        longitude -= twoPi;
      }

      double kmax = 20;
      double k = 0;
      double latitude = Math.Atan2(
        eci.z,
        Math.Sqrt((eci.x * eci.x) + (eci.y * eci.y)) 
      );

      double C = 0;
      while (k < kmax) {
        C = 1 / Math.Sqrt(1 - (e2 * (Math.Sin(latitude) * Math.Sin(latitude))));
        latitude = Math.Atan2(eci.z + (a * C * e2 * Math.Sin(latitude)), R);
        k += 1;
      }
      double height = (R / Math.Cos(latitude)) - (a * C);

      Geodetic geodetic = new Geodetic();
      geodetic.longitude = longitude;
      geodetic.latitude = latitude;
      geodetic.height = height;

      return geodetic;
    }

    public Coordinates ecfToEci(Coordinates ecf, double gmst) {
      // ccar.colorado.edu/ASEN5070/handouts/coordsys.doc
      //
      // [X]     [C -S  0][X]
      // [Y]  =  [S  C  0][Y]
      // [Z]eci  [0  0  1][Z]ecf
      //
      double X = (ecf.x * Math.Cos(gmst)) - (ecf.y * Math.Sin(gmst));
      double Y = (ecf.x * (Math.Sin(gmst))) + (ecf.y * Math.Cos(gmst));
      double Z = ecf.z;

      Coordinates eci = new Coordinates();
      eci.x = X;
      eci.y = Y;
      eci.z = Z;
      return eci;
    }

    public Coordinates eciToEcf(Coordinates eci, double gmst) {
      // ccar.colorado.edu/ASEN5070/handouts/coordsys.doc
      //
      // [X]     [C -S  0][X]
      // [Y]  =  [S  C  0][Y]
      // [Z]eci  [0  0  1][Z]ecf
      //
      //
      // Inverse:
      // [X]     [C  S  0][X]
      // [Y]  =  [-S C  0][Y]
      // [Z]ecf  [0  0  1][Z]eci

      double x = (eci.x * Math.Cos(gmst)) + (eci.y * Math.Sin(gmst));
      double y = (eci.x * (-Math.Sin(gmst))) + (eci.y * Math.Cos(gmst));
      // double { z } = eci;
      double z  = eci.z;  

      Coordinates ecf = new Coordinates();
      ecf.x = x;
      ecf.y = y;
      ecf.z = z;
      return ecf;

    }


    Topocentric topocentric(Geodetic observerGeodetic, Coordinates satelliteEcf) {
      // http://www.celestrak.com/columns/v02n02/
      // TS Kelso's method, except I'm using ECF frame
      // and he uses ECI.

      double longitude = observerGeodetic.longitude;
      double latitude = observerGeodetic.latitude;

      Coordinates observerEcf = geodeticToEcf(observerGeodetic);

      double rx = satelliteEcf.x - observerEcf.x;
      double ry = satelliteEcf.y - observerEcf.y;
      double rz = satelliteEcf.z - observerEcf.z;

      double topS = ((Math.Sin(latitude) * Math.Cos(longitude) * rx)
          + (Math.Sin(latitude) * Math.Sin(longitude) * ry))
        - (Math.Cos(latitude) * rz);

      double topE = (-Math.Sin(longitude) * rx)
        + (Math.Cos(longitude) * ry);

      double topZ = (Math.Cos(latitude) * Math.Cos(longitude) * rx)
        + (Math.Cos(latitude) * Math.Sin(longitude) * ry)
        + (Math.Sin(latitude) * rz);

      Topocentric topo = new Topocentric();
      topo.topS = topS;
      topo.topE = topE;
      topo.topZ = topZ;
      return topo;


    }





/**
 * @param {Object} tc
 * @param {Number} tc.topS Positive horizontal vector S due south.
 * @param {Number} tc.topE Positive horizontal vector E due east.
 * @param {Number} tc.topZ Vector Z normal to the surface of the earth (up).
 * @returns {Object}
 */

  public LookAngles topocentricToLookAngles( Topocentric tc) {
    
    double topS = tc.topS;
    double topE = tc.topE;
    double topZ = tc.topZ;

    double rangeSat = Math.Sqrt((topS * topS) + (topE * topE) + (topZ * topZ));
    double El = Math.Asin(topZ / rangeSat);
    double Az = Math.Atan2(-topE, topS) + pi;

    LookAngles lookAngles = new LookAngles();
    lookAngles.azimuth = Az;
    lookAngles.elevation = El;
    lookAngles.rangeSat = rangeSat;

    return lookAngles;

  }


  public LookAngles ecfToLookAngles(Geodetic observerGeodetic, Coordinates satelliteEcf) {
    Topocentric topocentricCoords = topocentric(observerGeodetic, satelliteEcf);
    return topocentricToLookAngles(topocentricCoords);
  }















  }

}