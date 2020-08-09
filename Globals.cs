using System;

namespace Satellite_cs{

  public class Globals {

    public double pi = Math.PI;
    public double twoPi = Math.PI * 2;
    public double deg2rad;
    public double rad2deg;
    public double minutesPerDay = 1440.0;

    public double mu = 398600.5; // in km3 / s2
    public double earthRadius = 6378.137; // in km
    public double xke;
    public double vkmpersec; 
    public double tumin;
    public double j2 = 0.00108262998905;
    public double j3 = -0.00000253215306;
    public double j4 = -0.00000161098761;
    public double j3oj2; //
    public double x2o3 = 2.0 / 3.0;

    
    public Globals(){
      deg2rad = pi / 180.0;
      rad2deg = 180 / pi;
      xke = 60.0 / Math.Sqrt((earthRadius * earthRadius * earthRadius) / mu);
      vkmpersec = (earthRadius * xke) / 60.0;
      tumin = 1.0 / xke;
      j3oj2 = j3 / j2;
    }

  }

}