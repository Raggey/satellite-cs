using System;

namespace Satellite_cs {
  public class Propagate {



    public PositionAndVelocity propagate(Satrec satrec, DateTime datetime) {

      Globals globals = new Globals();
      double minutesPerDay = globals.minutesPerDay;

      // Return a position and velocity vector for a given date and time.
      Ext ext = new Ext();
      double j = ext.jday(datetime);
      double m = (j - satrec.jdsatepoch) * minutesPerDay;
      
      Sgp4 sgp4 = new Sgp4();
      return sgp4.sgp4(satrec, m);
    }

  }

}