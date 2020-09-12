using System;
using System.ComponentModel;

namespace Satellite_cs
{
    class Program
    {
        static void Main(string[] args)
        {


        // string line0 = "ISS (ZARYA)    ";
        string line1 = "1 25544U 98067A   20206.38292522 -.00000985  00000-0 -95291-5 0  9998";
        string line2 = "2 25544  51.6430 164.3636 0001088 140.8410 323.1994 15.49511774237787";
        
        Sat_Io io = new Sat_Io();
        Satrec satrec = io.twoline2satrec(line1,line2);

        Sgp4 sgp4 = new Sgp4();
        PositionAndVelocity positionAndVelocity = sgp4.sgp4(satrec, 0);

        // TODO: Impl date
//         //  Or you can use a JavaScript Date
// var positionAndVelocity = satellite.propagate(satrec, new Date());

        // Set the Observer at 122.03 West by 36.96 North, in RADIANS
        Geodetic observerGd = new Geodetic();
        Transform tf = new Transform();
        DopplerFactor df = new DopplerFactor();

        observerGd.longitude = tf.degreesToRadians(-122.0308);
        observerGd.latitude = tf.degreesToRadians(36.9613422);
        observerGd.height = 0.370;


        Gstime gmst = new Gstime();
        DateTime newDate = DateTime.Now;
        double gmstTime = gmst.gstime(newDate); // GMST definition 

        DateTime UTCdate = newDate.ToUniversalTime();

        // // You can get ECF, Geodetic, Look Angles, and Doppler Factor.
        Coordinates positionEcf = tf.eciToEcf(positionAndVelocity.position_ECI, gmstTime);
        Coordinates observerEcf = tf.geodeticToEcf(observerGd);
        Coordinates velocityEcf = tf.eciToEcf(positionAndVelocity.velocity_ECI, gmstTime);
        Geodetic positionGd = tf.eciToGeodetic(positionAndVelocity.position_ECI, gmstTime);
        LookAngles lookAngles = tf.ecfToLookAngles(observerGd, positionEcf);
        double dopplerFactor = df.dopplerFactor(observerEcf, positionEcf, velocityEcf);

        Console.WriteLine(dopplerFactor);
        Console.WriteLine(lookAngles.azimuth);
        Console.WriteLine(lookAngles.elevation);
        Console.WriteLine(lookAngles.rangeSat);
        Console.WriteLine("Look i didnt crash");


        }
    }
}
