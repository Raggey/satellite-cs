using System;

namespace Satellite_cs
{
  public class DopplerFactor   {


      public double x;
      public double y;
      public double z;




      public double dopplerFactor(Coordinates location, Coordinates position, Coordinates velocity){


        double currentRange = Math.Sqrt(
          Math.Pow( (position.x - location.x), 2) +
          Math.Pow( (position.y - location.y), 2 ) + 
          Math.Pow( (position.z - location.z), 2 ));



        Coordinates nextPos = new Coordinates();
        nextPos.x =  position.x + velocity.x;
        nextPos.y = position.y + velocity.y;
        nextPos.z = position.z + velocity.z;

        double nextRange = Math.Sqrt(
          Math.Pow( (nextPos.x - location.x), 2) +
          Math.Pow( (nextPos.y - location.y), 2) +
          Math.Pow( (nextPos.z - location.z), 2));

        double rangeRate = nextRange - currentRange;

        rangeRate *= sign(rangeRate);
        double c = 299792.458; // Speed of light in km/s
        return (1 + (rangeRate / c));
      }


      private double sign(double value) {
        return value >= 0 ? 1 : -1;
      }





    }

}