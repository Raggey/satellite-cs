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

        DspaceOptions dspaceOptions = new DspaceOptions();

        dspaceOptions.irez = satrec.irez;
        dspaceOptions.d2201 = satrec.d2201;
        dspaceOptions.d2211 = satrec.d2211;
        dspaceOptions.d3210 = satrec.d3210;
        dspaceOptions.d3222 = satrec.d3222;
        dspaceOptions.d4410 = satrec.d4410;
        dspaceOptions.d4422 = satrec.d4422;
        dspaceOptions.d5220 = satrec.d5220;
        dspaceOptions.d5232 = satrec.d5232;
        dspaceOptions.d5421 = satrec.d5421;
        dspaceOptions.d5433 = satrec.d5433;
        dspaceOptions.dedt = satrec.dedt;
        dspaceOptions.del1 = satrec.del1;
        dspaceOptions.del2 = satrec.del2;
        dspaceOptions.del3 = satrec.del3;
        dspaceOptions.didt = satrec.didt;
        dspaceOptions.dmdt = satrec.dmdt;
        dspaceOptions.dnodt = satrec.dnodt;
        dspaceOptions.domdt = satrec.domdt;
        dspaceOptions.argpo = satrec.argpo;
        dspaceOptions.argpdot = satrec.argpdot;
        dspaceOptions.t = satrec.t;
        dspaceOptions.tc = tc;
        dspaceOptions.gsto = satrec.gsto;
        dspaceOptions.xfact = satrec.xfact;
        dspaceOptions.xlamo = satrec.xlamo;
        dspaceOptions.no = satrec.no;
        dspaceOptions.atime = satrec.atime;
        dspaceOptions.em = em;
        dspaceOptions.argpm = argpm;
        dspaceOptions.inclm = inclm;
        dspaceOptions.xli = satrec.xli;
        dspaceOptions.mm = mm;
        dspaceOptions.xni = satrec.xni;
        dspaceOptions.nodem = nodem;
        dspaceOptions.nm = nm;
          

        Dspace dspace = new Dspace();
        DspaceResults dspaceResults = new DspaceResults();
        dspaceResults = dspace.dspace(dspaceOptions);

        em = dspaceResults.em;
        argpm = dspaceResults.argpm;
        inclm = dspaceResults.inclm;
        mm = dspaceResults.mm;
        nodem = dspaceResults.nodem;
        nm = dspaceResults.nm;


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

       //  sgp4fix fix tolerance to avoid a divide by zero
      if (em < 1.0e-6) {
        em = 1.0e-6;
      }
      mm += satrec.no * templ;
      xlm = mm + argpm + nodem;

      nodem %= twoPi;
      argpm %= twoPi;
      xlm %= twoPi;
      mm = (xlm - argpm - nodem) % twoPi;

      // ----------------- compute extra mean quantities -------------
      double sinim = Math.Sin(inclm);
      double cosim = Math.Cos(inclm);

      // -------------------- add lunar-solar periodics --------------
      double ep = em;
      xincp = inclm;
      argpp = argpm;
      nodep = nodem;
      mp = mm;
      sinip = sinim;
      cosip = cosim;
      if (satrec.method == 'd') {

        DpperOptions dpperParameters = new DpperOptions();
        dpperParameters.inclo = satrec.inclo;
        dpperParameters.init = 'n';
        dpperParameters.ep = ep;
        dpperParameters.inclp = xincp;
        dpperParameters.nodep = nodep;
        dpperParameters.argpp = argpp;
        dpperParameters.mp = mp;
        dpperParameters.opsmode = satrec.operationmode;


        Dpper dpper = new Dpper();
        DpperResult dpperResult = new DpperResult();
        dpperResult = dpper.dpper(satrec, dpperParameters);
        ep = dpperResult.ep;
        nodep = dpperResult.nodep;
        argpp = dpperResult.argpp;
        mp = dpperResult.mp;

        xincp = dpperResult.inclp;

        if (xincp < 0.0) {
          xincp = -xincp;
          nodep += pi;
          argpp -= pi;
        }
        if (ep < 0.0 || ep > 1.0) {
          //  printf("// error ep %f\n", ep);
          satrec.error = 3;
          //  sgp4fix add return
          PositionAndVelocity noValue = new PositionAndVelocity();
          return noValue; //TODO set this to 0
        }

      }

      //  -------------------- long period periodics ------------------
      if (satrec.method == 'd') {
        sinip = Math.Sin(xincp);
        cosip = Math.Cos(xincp);
        satrec.aycof = -0.5 * j3oj2 * sinip;

        //  sgp4fix for divide by zero for xincp = 180 deg
        if (Math.Abs(cosip + 1.0) > 1.5e-12) {
          satrec.xlcof = (-0.25 * j3oj2 * sinip * (3.0 + (5.0 * cosip))) / (1.0 + cosip);
        } else {
          satrec.xlcof = (-0.25 * j3oj2 * sinip * (3.0 + (5.0 * cosip))) / temp4;
        }
      }

      double axnl = ep * Math.Cos(argpp);
      temp = 1.0 / (am * (1.0 - (ep * ep)));
      double aynl = (ep * Math.Sin(argpp)) + (temp * satrec.aycof);
      double xl = mp + argpp + nodep + (temp * satrec.xlcof * axnl);

      // --------------------- solve kepler's equation ---------------
      double u = (xl - nodep) % twoPi;
      eo1 = u;
      tem5 = 9999.9;
      double ktr = 1;

      //    sgp4fix for kepler iteration
      //    the following iteration needs better limits on corrections
      while (Math.Abs(tem5) >= 1.0e-12 && ktr <= 10) {
        sineo1 = Math.Sin(eo1);
        coseo1 = Math.Cos(eo1);
        tem5 = 1.0 - (coseo1 * axnl) - (sineo1 * aynl);
        tem5 = (((u - (aynl * coseo1)) + (axnl * sineo1)) - eo1) / tem5;
        if (Math.Abs(tem5) >= 0.95) {
          if (tem5 > 0.0) {
            tem5 = 0.95;
          } else {
            tem5 = -0.95;
          }
        }
        eo1 += tem5;
        ktr += 1;
      }
      
      //  ------------- short period preliminary quantities -----------
      double ecose = (axnl * coseo1) + (aynl * sineo1);
      double esine = (axnl * sineo1) - (aynl * coseo1);
      double el2 = (axnl * axnl) + (aynl * aynl);
      double pl = am * (1.0 - el2);
      if (pl < 0.0) {
        //  printf("// error pl %f\n", pl);
        satrec.error = 4;
        //  sgp4fix add return
        PositionAndVelocity noValue = new PositionAndVelocity();
        return noValue; //TODO set this to 0
      }

      double rl = am * (1.0 - ecose);
      double rdotl = (Math.Sqrt(am) * esine) / rl;
      double rvdotl = Math.Sqrt(pl) / rl;
      double betal = Math.Sqrt(1.0 - el2);
      temp = esine / (1.0 + betal);
      double sinu = (am / rl) * (sineo1 - aynl - (axnl * temp));
      double cosu = (am / rl) * ((coseo1 - axnl) + (aynl * temp));
      su = Math.Atan2(sinu, cosu);
      double sin2u = (cosu + cosu) * sinu;
      double cos2u = 1.0 - (2.0 * sinu * sinu);
      temp = 1.0 / pl;
      double temp1 = 0.5 * j2 * temp;
      double temp2 = temp1 * temp;

      // -------------- update for short period periodics ------------
      if (satrec.method == 'd') {
        cosisq = cosip * cosip;
        satrec.con41 = (3.0 * cosisq) - 1.0;
        satrec.x1mth2 = 1.0 - cosisq;
        satrec.x7thm1 = (7.0 * cosisq) - 1.0;
      }

      double mrt = (rl * (1.0 - (1.5 * temp2 * betal * satrec.con41)))
        + (0.5 * temp1 * satrec.x1mth2 * cos2u);

      // sgp4fix for decaying satellites
      if (mrt < 1.0) {
        // printf("// decay condition %11.6f \n",mrt);
        satrec.error = 6;
        // return {
        //   position: false,
        //   velocity: false,
        // };

        PositionAndVelocity noValue = new PositionAndVelocity();
        return noValue; //TODO set this to 0
      }

      su -= 0.25 * temp2 * satrec.x7thm1 * sin2u;
      double xnode = nodep + (1.5 * temp2 * cosip * sin2u);
      double xinc = xincp + (1.5 * temp2 * cosip * sinip * cos2u);
      double mvt = rdotl - ((nm * temp1 * satrec.x1mth2 * sin2u) / xke);
      double rvdot = rvdotl + ((nm * temp1 * ((satrec.x1mth2 * cos2u) + (1.5 * satrec.con41))) / xke);


      // --------------------- orientation vectors -------------------
      double sinsu = Math.Sin(su);
      double cossu = Math.Cos(su);
      double snod = Math.Sin(xnode);
      double cnod = Math.Cos(xnode);
      double sini = Math.Sin(xinc);
      double cosi = Math.Cos(xinc);
      double xmx = -snod * cosi;
      double xmy = cnod * cosi;
      double ux = (xmx * sinsu) + (cnod * cossu);
      double uy = (xmy * sinsu) + (snod * cossu);
      double uz = sini * sinsu;
      double vx = (xmx * cossu) - (cnod * sinsu);
      double vy = (xmy * cossu) - (snod * sinsu);
      double vz = sini * cossu;

      // --------- position and velocity (in km and km/sec) ----------

      PositionAndVelocity positionAndVelocity = new PositionAndVelocity();
     
      positionAndVelocity.position_ECI.x = (mrt * ux) * earthRadius;
      positionAndVelocity.position_ECI.y = (mrt * uy) * earthRadius;
      positionAndVelocity.position_ECI.z = (mrt * uz) * earthRadius;

      // double r = {
      //   x: (mrt * ux) * earthRadius,
      //   y: (mrt * uy) * earthRadius,
      //   z: (mrt * uz) * earthRadius,
      // };

      positionAndVelocity.velocity_ECI.x = ((mvt * ux) + (rvdot * vx)) * vkmpersec;
      positionAndVelocity.velocity_ECI.y = ((mvt * uy) + (rvdot * vy)) * vkmpersec;
      positionAndVelocity.velocity_ECI.z = ((mvt * uz) + (rvdot * vz)) * vkmpersec;



      // double v = {
      //   x: ((mvt * ux) + (rvdot * vx)) * vkmpersec,
      //   y: ((mvt * uy) + (rvdot * vy)) * vkmpersec,
      //   z: ((mvt * uz) + (rvdot * vz)) * vkmpersec,
      // };

      // return {
      //   position: r,
      //   velocity: v,
      // };


      return positionAndVelocity;

    }



  }


}
