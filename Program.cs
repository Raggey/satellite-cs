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
        
        Sat_Io io = new Sat_Io();
        Satrec satrec = io.twoline2rv(line1,line2);

        Console.WriteLine("Look i didnt crash");

        // Console.WriteLine(line1.Substring(50, 2) );

        Console.WriteLine(satrec.nddot);

        Console.WriteLine( satrec.bstar);


        // Tle iss = new Tle(line0,line1,line2); 

        // Console.WriteLine(iss.catalogNumber);
        // Console.WriteLine(iss.classification);
        // Console.WriteLine(iss.designator);
        // Console.WriteLine(iss.epoch);
        // Console.WriteLine(iss.meanMotion_fd);
        // Console.WriteLine(iss.meanMotion_sd);
        // Console.WriteLine(iss.dragTerm);
        // Console.WriteLine(iss.ephemerisType);


        // Console.WriteLine(iss.elementSetNumber);
        // Console.WriteLine(iss.checksum1);


        // Console.WriteLine(iss.inclination);
        // Console.WriteLine(iss.rightAscension);
        // Console.WriteLine(iss.eccentricity);
        // Console.WriteLine(iss.argumentPerigee);
        // Console.WriteLine(iss.meanAnomaly);
        // Console.WriteLine(iss.meanMotion);
        // Console.WriteLine(iss.revNumAtEpoch);
        // Console.WriteLine(iss.checksum2);


//         ISS DEB                 
// 1 44306U 98067QD  20206.84164985  .00003039  00000-0  52283-4 0  9991
// 2 44306  51.6395 152.6366 0007210 101.5333 258.6468 15.54606462 65585


        }
    }
}
