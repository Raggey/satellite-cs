using System;

namespace Satellite_cs{

  public class Ext{

/* -----------------------------------------------------------------------------
 *
 *                           procedure days2mdhms
 *
 *  this procedure converts the day of the year, days, to the equivalent month
 *    day, hour, minute and second.
 *
 *  algorithm     : set up array for the number of days per month
 *                  find leap year - use 1900 because 2000 is a leap year
 *                  loop through a temp value while the value is < the days
 *                  perform int conversions to the correct day and month
 *                  convert remainder into h m s using type conversions
 *
 *  author        : david vallado                  719-573-2600    1 mar 2001
 *
 *  inputs          description                    range / units
 *    year        - year                           1900 .. 2100
 *    days        - julian day of the year         0.0  .. 366.0
 *
 *  outputs       :
 *    mon         - month                          1 .. 12
 *    day         - day                            1 .. 28,29,30,31
 *    hr          - hour                           0 .. 23
 *    min         - minute                         0 .. 59
 *    sec         - second                         0.0 .. 59.999
 *
 *  locals        :
 *    dayofyr     - day of year
 *    temp        - temporary extended values
 *    inttemp     - temporary int value
 *    i           - index
 *    lmonth[12]  - int array containing the number of days per month
 *
 *  coupling      :
 *    none.
 * --------------------------------------------------------------------------- */


    public MdhmsResult days2mdhms(int year, float days ){

      MdhmsResult mdhmsResult = new MdhmsResult();

      int feb = ((year % 4) == 0 ? 29 : 28); // Account for leap year
      int[] lmonth = new int[] {31, feb, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
      double dayofyr = Math.Floor(days);

       //  ----------------- find month and day of month ----------------
      int i = 1;
      int inttemp = 0;
      while ((dayofyr > (inttemp + lmonth[i - 1])) && i < 12) {
        inttemp += lmonth[i - 1];
        i += 1;
      }

      int mon = i;
      double day = dayofyr - inttemp;

      //  ----------------- find hours minutes and seconds -------------
      double temp = (days - dayofyr) * 24.0;
      double hr = Math.Floor(temp);
      temp = (temp - hr) * 60.0;
      double minute = Math.Floor(temp);
      double sec = (temp - minute) * 60.0;


      // Set params in object and return

      mdhmsResult.mon = mon;
      mdhmsResult.day = day;
      mdhmsResult.hr = hr;
      mdhmsResult.minute = minute;
      mdhmsResult.sec = sec;

      // return {
      //   mon,
      //   day,
      //   hr,
      //   minute,
      //   sec,
      // };

      return mdhmsResult;

    }



/* -----------------------------------------------------------------------------
 *
 *                           procedure jday
 *
 *  this procedure finds the julian date given the year, month, day, and time.
 *    the julian date is defined by each elapsed day since noon, jan 1, 4713 bc.
 *
 *  algorithm     : calculate the answer in one step for efficiency
 *
 *  author        : david vallado                  719-573-2600    1 mar 2001
 *
 *  inputs          description                    range / units
 *    year        - year                           1900 .. 2100
 *    mon         - month                          1 .. 12
 *    day         - day                            1 .. 28,29,30,31
 *    hr          - universal time hour            0 .. 23
 *    min         - universal time min             0 .. 59
 *    sec         - universal time sec             0.0 .. 59.999
 *
 *  outputs       :
 *    jd          - julian date                    days from 4713 bc
 *
 *  locals        :
 *    none.
 *
 *  coupling      :
 *    none.
 *
 *  references    :
 *    vallado       2007, 189, alg 14, ex 3-14
 *
 * --------------------------------------------------------------------------- */

    public double jday(int year, int mon, double day, double hr, double minute, double sec){
        //if (year instanceof Date) {
        //   const date = year;
        //   return jdayInternal(
        //     date.getUTCFullYear(),
        //     date.getUTCMonth() + 1, // Note, this function requires months in range 1-12.
        //     date.getUTCDate(),
        //     date.getUTCHours(),
        //     date.getUTCMinutes(),
        //     date.getUTCSeconds(),
        //     date.getUTCMilliseconds(),
        //   );
        // }

      return jdayInternal(year, mon, day, hr, minute, sec);
    }

    public double jdayInternal(int year, int mon, double day, double hr, double minute, double sec) {
      double msec = 0;
      return (
        ((367.0 * year) - Math.Floor((7 * (year + Math.Floor((mon + 9) / 12.0))) * 0.25))
        + Math.Floor((275 * mon) / 9.0)
        + day + 1721013.5
        + (((((msec / 60000) + (sec / 60.0) + minute) / 60.0) + hr) / 24.0) // ut in days
        // # - 0.5*sgn(100.0*year + mon - 190002.5) + 0.5;
      );
    }



    public Ext(){

    }

    


  }

}