using System;
using System.ComponentModel;

namespace Satellite_cs
{
    class Program
    {
        static void Main(string[] args)
        {


        // string line0 = "ISS (ZARYA)    ";
        string line1 = "1 25544U 98067A   20258.18510567  .00000068  00000-0  93716-5 0  9995";
        string line2 = "2 25544  51.6438 268.0523 0000907  94.9123  25.5188 15.48946285245810";
        
        
        Satellite_cs iss = new Satellite_cs(line1,line2);
  
        Console.WriteLine("Long: " + iss.longitudeStr);
        Console.WriteLine("Lat: " + iss.latitudeStr);

        }
    }
}
