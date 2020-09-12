using System;


namespace Satellite_cs {

  /* -----------------------------------------------------------------------------
  *
  *                           function gstime
  *
  *  this function finds the greenwich sidereal time.
  *
  *  author        : david vallado                  719-573-2600    1 mar 2001
  *
  *  inputs          description                    range / units
  *    jdut1       - julian date in ut1             days from 4713 bc
  *
  *  outputs       :
  *    gstime      - greenwich sidereal time        0 to 2pi rad
  *
  *  locals        :
  *    temp        - temporary variable for doubles   rad
  *    tut1        - julian centuries from the
  *                  jan 1, 2000 12 h epoch (ut1)
  *
  *  coupling      :
  *    none
  *
  *  references    :
  *    vallado       2004, 191, eq 3-45
  * --------------------------------------------------------------------------- */
  

  
  public class Gstime {
   
    double deg2rad;
    double twoPi;



    public Gstime() {

      Globals globals = new Globals();
      deg2rad = globals.deg2rad;
      twoPi = globals.twoPi;

    }


    // TODO: Verify this function against the satellite-js version
    public double gstime(DateTime dateTime){
      // Give this a jday
      Ext ext = new Ext();

      double jday = ext.jday(dateTime); //convert the time to julian time 

      return gstimeInternal(jday);
    }


    public double gstimeInternal(double jdut1){

      double tut1 = (jdut1 - 2451545.0) / 36525.0;

      double temp = (-6.2e-6 * tut1 * tut1 * tut1)
        + (0.093104 * tut1 * tut1)
        + (((876600.0 * 3600) + 8640184.812866) * tut1) + 67310.54841; // # sec
      temp = ((temp * deg2rad) / 240.0) % twoPi; // 360/86400 = 1/240, to deg, to rad

      //  ------------------------ check quadrants ---------------------
      if (temp < 0.0) {
        temp += twoPi;
      }

      return temp;
    }

  }


}
