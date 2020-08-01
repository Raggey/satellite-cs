using System;

namespace Satellite_cs{

  public class Initl{
    
    public Initl(){

    }
    
    /*-----------------------------------------------------------------------------
 *
 *                           procedure initl
 *
 *  this procedure initializes the sgp4 propagator. all the initialization is
 *    consolidated here instead of having multiple loops inside other routines.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    ecco        - eccentricity                           0.0 - 1.0
 *    epoch       - epoch time in days from jan 0, 1950. 0 hr
 *    inclo       - inclination of satellite
 *    no          - mean motion of satellite
 *    satn        - satellite number
 *
 *  outputs       :
 *    ainv        - 1.0 / a
 *    ao          - semi major axis
 *    con41       -
 *    con42       - 1.0 - 5.0 cos(i)
 *    cosio       - cosine of inclination
 *    cosio2      - cosio squared
 *    eccsq       - eccentricity squared
 *    method      - flag for deep space                    'd', 'n'
 *    omeosq      - 1.0 - ecco * ecco
 *    posq        - semi-parameter squared
 *    rp          - radius of perigee
 *    rteosq      - square root of (1.0 - ecco*ecco)
 *    sinio       - sine of inclination
 *    gsto        - gst at time of observation               rad
 *    no          - mean motion of satellite
 *
 *  locals        :
 *    ak          -
 *    d1          -
 *    del         -
 *    adel        -
 *    po          -
 *
 *  coupling      :
 *    getgravconst
 *    gstime      - find greenwich sidereal time from the julian date
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report #3 1980
 *    hoots, norad spacetrack report #6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/

    public InitlResult initl(InitlOptions initlOptions){

      Globals globals = new Globals();
      double twoPi = globals.twoPi;
      double xke = globals.xke;
      double j2 = globals.j2;
      double x2o3 = globals.x2o3;

      double ecco = initlOptions.ecco;
      double epoch = initlOptions.epoch;
      double inclo = initlOptions.inlco;
      double opsmode = initlOptions.opsmode;

      // let {
      //   no,
      // } = options;
      double no = initlOptions.no;

      // sgp4fix use old way of finding gst
      // ----------------------- earth constants ---------------------
      // sgp4fix identify constants and allow alternate values

       // ------------- calculate auxillary epoch quantities ----------
      double eccsq = ecco * ecco;
      double omeosq = 1.0 - eccsq;
      double rteosq = Math.Sqrt(omeosq);
      double cosio = Math.Cos(inclo);
      double cosio2 = cosio * cosio;

      // ------------------ un-kozai the mean motion -----------------
      // double ak = ((xke / no) ** x2o3);
      double ak =  Math.Pow( (xke / no) , x2o3);
      double d1 = (0.75 * j2 * ((3.0 * cosio2) - 1.0)) / (rteosq * omeosq);
      double delPrime = d1 / (ak * ak);
      double adel = ak * (1.0 - (delPrime * delPrime)
        - (delPrime * ((1.0 / 3.0) + ((134.0 * delPrime * delPrime) / 81.0))));
      delPrime = d1 / (adel * adel);
      no /= (1.0 + delPrime);

      // double ao = ((xke / no) ** x2o3);
      double ao =  Math.Pow((xke / no) , x2o3);
      double sinio = Math.Sin(inclo);
      double po = ao * omeosq;
      double con42 = 1.0 - (5.0 * cosio2);
      double con41 = -con42 - cosio2 - cosio2;
      double ainv = 1.0 / ao;
      double posq = po * po;
      double rp = ao * (1.0 - ecco);
      double method = 'n';


      //  sgp4fix modern approach to finding sidereal time
      double gsto;
      if (opsmode == 'a') {
        //  sgp4fix use old way of finding gst
        //  count integer number of days from 0 jan 1970
        double ts70 = epoch - 7305.0;
        double ds70 = Math.Floor(ts70 + 1.0e-8);
        double tfrac = ts70 - ds70;

        //  find greenwich location at epoch
        double c1 = 1.72027916940703639e-2;
        double thgr70 = 1.7321343856509374;
        double fk5r = 5.07551419432269442e-15;
        double c1p2p = c1 + twoPi;
        gsto = (thgr70 + (c1 * ds70) + (c1p2p * tfrac) + (ts70 * ts70 * fk5r)) % twoPi;
        if (gsto < 0.0) {
          gsto += twoPi;
        }
      } else {

        gsto = 0; //placeholder until gstime implemented;
        // gsto = gstime(epoch + 2433281.5);
      }


      // set variables and return object
      InitlResult initlResult = new InitlResult();
      initlResult.no = no;
      initlResult.method = method;

      initlResult.ainv = ainv;
      initlResult.ao = ao;
      initlResult.con41 = con41;
      initlResult.con42 = con42;
      initlResult.cosio = cosio;

      initlResult.cosio2 = cosio2;
      initlResult.eccsq = eccsq;
      initlResult.omeosq = omeosq;
      initlResult.posq = posq;

      initlResult.rp = rp;
      initlResult.rteosq = rteosq;
      initlResult.sinio = sinio;
      initlResult.gsto = gsto;
     
      return initlResult;
      


    }


  }

}