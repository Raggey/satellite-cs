using System;

namespace Satellite_cs
{
    class Program
    {
        static void Main(string[] args)
        {


        string line0 = "ISS (ZARYA)    ";
        string line1 = "1 25544U 98067A   20206.38292522 -.00000985  00000-0 -95291-5 0  9998";
        string line2 = "2 25544  51.6430 164.3636 0001088 140.8410 323.1994 15.49511774237787";

        Tle iss = new Tle(line0,line1,line2); 

        Console.WriteLine(iss.line1.ToString());

         foreach(String s in iss.line1) 
        { 
            Console.WriteLine(s); 
        } 

        Console.WriteLine(iss.catalogNumber);
        Console.WriteLine(iss.classification);
        Console.WriteLine(iss.designator);
        Console.WriteLine(iss.epoch);
        Console.WriteLine(iss.meanMotion_fd);
        Console.WriteLine(iss.meanMotion_sd);
        Console.WriteLine(iss.dragTerm);
        Console.WriteLine(iss.ephemerisType);
        Console.WriteLine(iss.elementNoCheckSum);


        }
    }
}
