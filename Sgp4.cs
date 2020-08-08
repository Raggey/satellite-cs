using System;


namespace Satellite_cs{


  public class Sgp4{


    public Sgp4(){

    }


/*----------------------------------------------------------------------------
 *
 *                             procedure sgp4
 *
 *  this procedure is the sgp4 prediction model from space command. this is an
 *    updated and combined version of sgp4 and sdp4, which were originally
 *    published separately in spacetrack report //3. this version follows the
 *    methodology from the aiaa paper (2006) describing the history and
 *    development of the code.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    satrec  - initialised structure from sgp4init() call.
 *    tsince  - time since epoch (minutes)
 *
 *  outputs       :
 *    r           - position vector                     km
 *    v           - velocity                            km/sec
 *  return code - non-zero on error.
 *                   1 - mean elements, ecc >= 1.0 or ecc < -0.001 or a < 0.95 er
 *                   2 - mean motion less than 0.0
 *                   3 - pert elements, ecc < 0.0  or  ecc > 1.0
 *                   4 - semi-latus rectum < 0.0
 *                   5 - epoch elements are sub-orbital
 *                   6 - satellite has decayed
 *
 *  locals        :
 *    am          -
 *    axnl, aynl        -
 *    betal       -
 *    cosim   , sinim   , cosomm  , sinomm  , cnod    , snod    , cos2u   ,
 *    sin2u   , coseo1  , sineo1  , cosi    , sini    , cosip   , sinip   ,
 *    cosisq  , cossu   , sinsu   , cosu    , sinu
 *    delm        -
 *    delomg      -
 *    dndt        -
 *    eccm        -
 *    emsq        -
 *    ecose       -
 *    el2         -
 *    eo1         -
 *    eccp        -
 *    esine       -
 *    argpm       -
 *    argpp       -
 *    omgadf      -
 *    pl          -
 *    r           -
 *    rtemsq      -
 *    rdotl       -
 *    rl          -
 *    rvdot       -
 *    rvdotl      -
 *    su          -
 *    t2  , t3   , t4    , tc
 *    tem5, temp , temp1 , temp2  , tempa  , tempe  , templ
 *    u   , ux   , uy    , uz     , vx     , vy     , vz
 *    inclm       - inclination
 *    mm          - mean anomaly
 *    nm          - mean motion
 *    nodem       - right asc of ascending node
 *    xinc        -
 *    xincp       -
 *    xl          -
 *    xlm         -
 *    mp          -
 *    xmdf        -
 *    xmx         -
 *    xmy         -
 *    nodedf      -
 *    xnode       -
 *    nodep       -
 *    np          -
 *
 *  coupling      :
 *    getgravconst-
 *    dpper
 *    dspace
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report //3 1980
 *    hoots, norad spacetrack report //6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/


    public PositionAndVelocity sgp4(Satrec satrec, double tsince){

      Globals globals = new Globals();

      double pi = globals.pi;
      double twoPi = globals.twoPi;
      double earthRadius = globals.earthRadius;
      double xke = globals.xke;
      double vkmpersec = globals.vkmpersec;
      double j2 = globals.j2;
      double j3oj2 = globals.j3oj2;
      double x2o3 = globals.x2o3;

      double coseo1 = 0.0;
      double sineo1 = 0.0;
      double cosip = 0.0;
      double sinip = 0.0;
      double cosisq = 0.0;
      double delm = 0.0;
      double delomg = 0.0;
      double eo1 = 0.0;
      double argpm = 0.0;
      double argpp = 0.0;
      double su = 0.0;
      double t3 = 0.0;
      double t4 = 0.0;
      double tc = 0.0;
      double tem5 = 0.0;
      double temp = 0.0;
      double tempa = 0.0;
      double tempe = 0.0;
      double templ = 0.0;
      double inclm = 0.0;
      double mm = 0.0;
      double nm = 0.0;
      double nodem = 0.0;
      double xincp = 0.0;
      double xlm = 0.0;
      double mp = 0.0;
      double nodep = 0.0;

      /* ------------------ set mathematical constants --------------- */
      // sgp4fix divisor for divide by zero check on inclination
      // the old check used 1.0 + cos(pi-1.0e-9), but then compared it to
      // 1.5 e-12, so the threshold was changed to 1.5e-12 for consistency

      double temp4 = 1.5e-12;

      // --------------------- clear sgp4 error flag -----------------
      satrec.t = tsince;
      satrec.error = 0;

      //  ------- update for secular gravity and atmospheric drag -----
      double xmdf = satrec.mo + (satrec.mdot * satrec.t);
      double argpdf = satrec.argpo + (satrec.argpdot * satrec.t);
      double nodedf = satrec.nodeo + (satrec.nodedot * satrec.t);
      argpm = argpdf;
      mm = xmdf;
      double t2 = satrec.t * satrec.t;
      nodem = nodedf + (satrec.nodecf * t2);
      tempa = 1.0 - (satrec.cc1 * satrec.t);
      tempe = satrec.bstar * satrec.cc4 * satrec.t;
      templ = satrec.t2cof * t2;

      if (satrec.isimp != 1) {
        delomg = satrec.omgcof * satrec.t;
        //  sgp4fix use mutliply for speed instead of pow
        double delmtemp = 1.0 + (satrec.eta * Math.Cos(xmdf));
        delm = satrec.xmcof * ((delmtemp * delmtemp * delmtemp) - satrec.delmo);
        temp = delomg + delm;
        mm = xmdf + temp;
        argpm = argpdf - temp;
        t3 = t2 * satrec.t;
        t4 = t3 * satrec.t;
        tempa = tempa - (satrec.d2 * t2) - (satrec.d3 * t3) - (satrec.d4 * t4);
        tempe += satrec.bstar * satrec.cc5 * (Math.Sin(mm) - satrec.sinmao);
        templ = templ + (satrec.t3cof * t3) + (t4 * (satrec.t4cof + (satrec.t * satrec.t5cof)));
      }

      nm = satrec.no;
      double em = satrec.ecco;
      inclm = satrec.inclo;

      if (satrec.method == 'd') {
        tc = satrec.t;

        // dspaceOptions

      }

      if (nm <= 0.0) {
        // printf("// error nm %f\n", nm);
        satrec.error = 2;
        // sgp4fix add return
        PositionAndVelocity noValue = new PositionAndVelocity();
        return noValue; //TODO set this to 0
      }

      double am = (  Math.Pow( (xke / nm) , x2o3)) * tempa * tempa;
      nm = xke / (   Math.Pow(am,1.5) );
      em -= tempe;

      // fix tolerance for error recognition
      // sgp4fix am is fixed from the previous nm check
      if (em >= 1.0 || em < -0.001) { // || (am < 0.95)
        // printf("// error em %f\n", em);
        satrec.error = 1;
        PositionAndVelocity noValue = new PositionAndVelocity();
        return noValue; //TODO set this to 0
      }











      
      PositionAndVelocity positionAndVelocity = new PositionAndVelocity();
      return positionAndVelocity;

    }



  }


}
