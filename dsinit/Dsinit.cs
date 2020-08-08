using System;

namespace Satellite_cs{


  public class Dsinit{



    public Dsinit(){

    }


/*-----------------------------------------------------------------------------
 *
 *                           procedure dsinit
 *
 *  this procedure provides deep space contributions to mean motion dot due
 *    to geopotential resonance with half day and one day orbits.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    cosim, sinim-
 *    emsq        - eccentricity squared
 *    argpo       - argument of perigee
 *    s1, s2, s3, s4, s5      -
 *    ss1, ss2, ss3, ss4, ss5 -
 *    sz1, sz3, sz11, sz13, sz21, sz23, sz31, sz33 -
 *    t           - time
 *    tc          -
 *    gsto        - greenwich sidereal time                   rad
 *    mo          - mean anomaly
 *    mdot        - mean anomaly dot (rate)
 *    no          - mean motion
 *    nodeo       - right ascension of ascending node
 *    nodedot     - right ascension of ascending node dot (rate)
 *    xpidot      -
 *    z1, z3, z11, z13, z21, z23, z31, z33 -
 *    eccm        - eccentricity
 *    argpm       - argument of perigee
 *    inclm       - inclination
 *    mm          - mean anomaly
 *    xn          - mean motion
 *    nodem       - right ascension of ascending node
 *
 *  outputs       :
 *    em          - eccentricity
 *    argpm       - argument of perigee
 *    inclm       - inclination
 *    mm          - mean anomaly
 *    nm          - mean motion
 *    nodem       - right ascension of ascending node
 *    irez        - flag for resonance           0-none, 1-one day, 2-half day
 *    atime       -
 *    d2201, d2211, d3210, d3222, d4410, d4422, d5220, d5232, d5421, d5433    -
 *    dedt        -
 *    didt        -
 *    dmdt        -
 *    dndt        -
 *    dnodt       -
 *    domdt       -
 *    del1, del2, del3        -
 *    ses  , sghl , sghs , sgs  , shl  , shs  , sis  , sls
 *    theta       -
 *    xfact       -
 *    xlamo       -
 *    xli         -
 *    xni
 *
 *  locals        :
 *    ainv2       -
 *    aonv        -
 *    cosisq      -
 *    eoc         -
 *    f220, f221, f311, f321, f322, f330, f441, f442, f522, f523, f542, f543  -
 *    g200, g201, g211, g300, g310, g322, g410, g422, g520, g521, g532, g533  -
 *    sini2       -
 *    temp        -
 *    temp1       -
 *    theta       -
 *    xno2        -
 *
 *  coupling      :
 *    getgravconst
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report #3 1980
 *    hoots, norad spacetrack report #6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/
    public DsinitResult dsinit(DsinitOptions dsinitOptions){

      Globals globals = new Globals();
      double pi = globals.pi;
      double twoPi = globals.twoPi;
      double xke = globals.xke;
      double x2o3 = globals.x2o3;

      double cosim = dsinitOptions.cosim;
      double argpo = dsinitOptions.argpo;
      double s1 = dsinitOptions.s1;
      double s2 = dsinitOptions.s2;
      double s3 = dsinitOptions.s3;
      double s4 = dsinitOptions.s4;
      double s5 = dsinitOptions.s5;
      double sinim = dsinitOptions.sinim;
      double ss1 = dsinitOptions.ss1;
      double ss2 = dsinitOptions.ss2;
      double ss3 = dsinitOptions.ss3;
      double ss4 = dsinitOptions.ss4;
      double ss5 = dsinitOptions.ss5;
      double sz1 = dsinitOptions.sz1;
      double sz3 = dsinitOptions.sz3;
      double sz11 = dsinitOptions.sz11;
      double sz13 = dsinitOptions.sz13;
      double sz21 = dsinitOptions.sz21;
      double sz23 = dsinitOptions.sz23;
      double sz31 = dsinitOptions.sz31;
      double sz33 = dsinitOptions.sz33;
      double t = dsinitOptions.t;
      double tc = dsinitOptions.tc;
      double gsto = dsinitOptions.gsto;
      double mo = dsinitOptions.mo;
      double mdot = dsinitOptions.mdot;
      double no = dsinitOptions.no;
      double nodeo = dsinitOptions.nodeo;
      double nodedot = dsinitOptions.nodedot;
      double xpidot = dsinitOptions.xpidot;
      double z1 = dsinitOptions.z1;
      double z3 = dsinitOptions.z3;
      double z11 = dsinitOptions.z11;
      double z13 = dsinitOptions.z13;
      double z21 = dsinitOptions.z21;
      double z23 = dsinitOptions.z23;
      double z31 = dsinitOptions.z31;
      double z33 = dsinitOptions.z33;
      double ecco = dsinitOptions.ecco;
      double eccsq = dsinitOptions.eccsq;

      double emsq = dsinitOptions.emsq;
      double em = dsinitOptions.em;
      double argpm = dsinitOptions.argpm;
      double inclm = dsinitOptions.inclm;
      double mm = dsinitOptions.mm;
      double nm = dsinitOptions.nm;
      double nodem = dsinitOptions.nodem;
      double irez = dsinitOptions.irez;
      double atime = dsinitOptions.atime;
      double d2201 = dsinitOptions.d2201;
      double d2211 = dsinitOptions.d2211;
      double d3210 = dsinitOptions.d3210;
      double d3222 = dsinitOptions.d3222;
      double d4410 = dsinitOptions.d4410;
      double d4422 = dsinitOptions.d4422;
      double d5220 = dsinitOptions.d5220;
      double d5232 = dsinitOptions.d5232;
      double d5421 = dsinitOptions.d5421;
      double d5433 = dsinitOptions.d5433;
      double dedt = dsinitOptions.dedt;
      double didt = dsinitOptions.didt;
      double dmdt = dsinitOptions.dmdt;
      double dnodt = dsinitOptions.dnodt; 
      double domdt = dsinitOptions.domdt;
      double del1 = dsinitOptions.del1;
      double del2 = dsinitOptions.del2;
      double del3 = dsinitOptions.del3;
      double xfact = dsinitOptions.xfact;
      double xlamo = dsinitOptions.xlamo;
      double xli = dsinitOptions.xli;
      double xni = dsinitOptions.xni;

      double f220 = 0.0;
      double f221 = 0.0;
      double f311 = 0.0;
      double f321 = 0.0;
      double f322 = 0.0;
      double f330 = 0.0;
      double f441 = 0.0;
      double f442 = 0.0;
      double f522 = 0.0;
      double f523 = 0.0;
      double f542 = 0.0;
      double f543 = 0.0;
      double g200 = 0.0;
      double g201 = 0.0;
      double g211 = 0.0;
      double g300 = 0.0;
      double g310 = 0.0;
      double g322 = 0.0;
      double g410 = 0.0;
      double g422 = 0.0;
      double g520 = 0.0;
      double g521 = 0.0;
      double g532 = 0.0;
      double g533 = 0.0;
      double sini2 = 0.0;
      double temp = 0.0;
      double temp1 = 0.0;
      double xno2 = 0.0;
      double ainv2 = 0.0;
      double aonv = 0.0;
      double cosisq = 0.0;
      double eoc = 0.0;

      double q22 = 1.7891679e-6;
      double q31 = 2.1460748e-6;
      double q33 = 2.2123015e-7;
      double root22 = 1.7891679e-6;
      double root44 = 7.3636953e-9;
      double root54 = 2.1765803e-9;
      double rptim = 4.37526908801129966e-3; // equates to 7.29211514668855e-5 rad/sec
      double root32 = 3.7393792e-7;
      double root52 = 1.1428639e-7;
      double znl = 1.5835218e-4;
      double zns = 1.19459e-5;

      // -------------------- deep space initialization ------------
      irez = 0;
      if ((nm < 0.0052359877) && (nm > 0.0034906585)) {
        irez = 1;
      }
      if ((nm >= 8.26e-3) && (nm <= 9.24e-3) && (em >= 0.5)) {
        irez = 2;
      }

      // ------------------------ do solar terms -------------------
      double ses = ss1 * zns * ss5;
      double sis = ss2 * zns * (sz11 + sz13);
      double sls = -zns * ss3 * ((sz1 + sz3) - 14.0 - (6.0 * emsq));
      double sghs = ss4 * zns * ((sz31 + sz33) - 6.0);
      double shs = -zns * ss2 * (sz21 + sz23);

      // sgp4fix for 180 deg incl
      if (inclm < 5.2359877e-2 || inclm > pi - 5.2359877e-2) {
        shs = 0.0;
      }
      if (sinim != 0.0) {
        shs /= sinim;
      }
      double sgs = sghs - (cosim * shs);

       // ------------------------- do lunar terms ------------------
      dedt = ses + (s1 * znl * s5);
      didt = sis + (s2 * znl * (z11 + z13));
      dmdt = sls - (znl * s3 * ((z1 + z3) - 14.0 - (6.0 * emsq)));
      double sghl = s4 * znl * ((z31 + z33) - 6.0);
      double shll = -znl * s2 * (z21 + z23);

      
      // sgp4fix for 180 deg incl
      if ((inclm < 5.2359877e-2) || (inclm > (pi - 5.2359877e-2))) {
        shll = 0.0;
      }
      domdt = sgs + sghl;
      dnodt = shs;
      if (sinim != 0.0) {
        domdt -= (cosim / sinim) * shll;
        dnodt += shll / sinim;
      }

      // ----------- calculate deep space resonance effects --------
      double dndt = 0.0;
      double theta = (gsto + (tc * rptim)) % twoPi;
      em += dedt * t;
      inclm += didt * t;
      argpm += domdt * t;
      nodem += dnodt * t;
      mm += dmdt * t;

      // sgp4fix for negative inclinations
      // the following if statement should be commented out
      // if (inclm < 0.0)
      // {
      //   inclm  = -inclm;
      //   argpm  = argpm - pi;
      //   nodem = nodem + pi;
      // }

      // -------------- initialize the resonance terms -------------
      if (irez != 0) {
        aonv = ( Math.Pow(  (nm / xke) , x2o3  ) );

        // ---------- geopotential resonance for 12 hour orbits ------
        if (irez == 2) {
          cosisq = cosim * cosim;
          double emo = em;
          em = ecco;
          double emsqo = emsq;
          emsq = eccsq;
          eoc = em * emsq;
          g201 = -0.306 - ((em - 0.64) * 0.440);

          if (em <= 0.65) {
            g211 = (3.616 - (13.2470 * em)) + (16.2900 * emsq);
            g310 = ((-19.302 + (117.3900 * em)) - (228.4190 * emsq)) + (156.5910 * eoc);
            g322 = ((-18.9068 + (109.7927 * em)) - (214.6334 * emsq)) + (146.5816 * eoc);
            g410 = ((-41.122 + (242.6940 * em)) - (471.0940 * emsq)) + (313.9530 * eoc);
            g422 = ((-146.407 + (841.8800 * em)) - (1629.014 * emsq)) + (1083.4350 * eoc);
            g520 = ((-532.114 + (3017.977 * em)) - (5740.032 * emsq)) + (3708.2760 * eoc);
          } else {
            g211 = ((-72.099 + (331.819 * em)) - (508.738 * emsq)) + (266.724 * eoc);
            g310 = ((-346.844 + (1582.851 * em)) - (2415.925 * emsq)) + (1246.113 * eoc);
            g322 = ((-342.585 + (1554.908 * em)) - (2366.899 * emsq)) + (1215.972 * eoc);
            g410 = ((-1052.797 + (4758.686 * em)) - (7193.992 * emsq)) + (3651.957 * eoc);
            g422 = ((-3581.690 + (16178.110 * em)) - (24462.770 * emsq)) + (12422.520 * eoc);
            if (em > 0.715) {
              g520 = ((-5149.66 + (29936.92 * em)) - (54087.36 * emsq)) + (31324.56 * eoc);
            } else {
              g520 = (1464.74 - (4664.75 * em)) + (3763.64 * emsq);
            }
          }
          if (em < 0.7) {
            g533 = ((-919.22770 + (4988.6100 * em)) - (9064.7700 * emsq)) + (5542.21 * eoc);
            g521 = ((-822.71072 + (4568.6173 * em)) - (8491.4146 * emsq)) + (5337.524 * eoc);
            g532 = ((-853.66600 + (4690.2500 * em)) - (8624.7700 * emsq)) + (5341.4 * eoc);
          } else {
            g533 = ((-37995.780 + (161616.52 * em)) - (229838.20 * emsq)) + (109377.94 * eoc);
            g521 = ((-51752.104 + (218913.95 * em)) - (309468.16 * emsq)) + (146349.42 * eoc);
            g532 = ((-40023.880 + (170470.89 * em)) - (242699.48 * emsq)) + (115605.82 * eoc);
          }
          sini2 = sinim * sinim;
          f220 = 0.75 * (1.0 + (2.0 * cosim) + cosisq);
          f221 = 1.5 * sini2;
          f321 = 1.875 * sinim * (1.0 - (2.0 * cosim) - (3.0 * cosisq));
          f322 = -1.875 * sinim * ((1.0 + (2.0 * cosim)) - (3.0 * cosisq));
          f441 = 35.0 * sini2 * f220;
          f442 = 39.3750 * sini2 * sini2;

          f522 = 9.84375 * sinim * ((sini2 * (1.0 - (2.0 * cosim) - (5.0 * cosisq)))
            + (0.33333333 * (-2.0 + (4.0 * cosim) + (6.0 * cosisq))));
          f523 = sinim * ((4.92187512 * sini2 * ((-2.0 - (4.0 * cosim)) + (10.0 * cosisq)))
            + (6.56250012 * ((1.0 + (2.0 * cosim)) - (3.0 * cosisq))));
          f542 = 29.53125 * sinim * ((2.0 - (8.0 * cosim))
            + (cosisq * (-12.0 + (8.0 * cosim) + (10.0 * cosisq))));
          f543 = 29.53125 * sinim * ((-2.0 - (8.0 * cosim))
            + (cosisq * ((12.0 + (8.0 * cosim)) - (10.0 * cosisq))));

          xno2 = nm * nm;
          ainv2 = aonv * aonv;
          temp1 = 3.0 * xno2 * ainv2;
          temp = temp1 * root22;
          d2201 = temp * f220 * g201;
          d2211 = temp * f221 * g211;
          temp1 *= aonv;
          temp = temp1 * root32;
          d3210 = temp * f321 * g310;
          d3222 = temp * f322 * g322;
          temp1 *= aonv;
          temp = 2.0 * temp1 * root44;
          d4410 = temp * f441 * g410;
          d4422 = temp * f442 * g422;
          temp1 *= aonv;
          temp = temp1 * root52;
          d5220 = temp * f522 * g520;
          d5232 = temp * f523 * g532;
          temp = 2.0 * temp1 * root54;
          d5421 = temp * f542 * g521;
          d5433 = temp * f543 * g533;
          xlamo = ((mo + nodeo + nodeo) - (theta + theta)) % twoPi;
          xfact = (mdot + dmdt + (2.0 * ((nodedot + dnodt) - rptim))) - no;
          em = emo;
          emsq = emsqo;
        }

        //  ---------------- synchronous resonance terms --------------
        if (irez == 1) {
          g200 = 1.0 + (emsq * (-2.5 + (0.8125 * emsq)));
          g310 = 1.0 + (2.0 * emsq);
          g300 = 1.0 + (emsq * (-6.0 + (6.60937 * emsq)));
          f220 = 0.75 * (1.0 + cosim) * (1.0 + cosim);
          f311 = (0.9375 * sinim * sinim * (1.0 + (3.0 * cosim))) - (0.75 * (1.0 + cosim));
          f330 = 1.0 + cosim;
          f330 *= 1.875 * f330 * f330;
          del1 = 3.0 * nm * nm * aonv * aonv;
          del2 = 2.0 * del1 * f220 * g200 * q22;
          del3 = 3.0 * del1 * f330 * g300 * q33 * aonv;
          del1 = del1 * f311 * g310 * q31 * aonv;
          xlamo = ((mo + nodeo + argpo) - theta) % twoPi;
          xfact = (mdot + xpidot + dmdt + domdt + dnodt) - (no + rptim);
        }

        //  ------------ for sgp4, initialize the integrator ----------
        xli = xlamo;
        xni = no;
        atime = 0.0;
        nm = no + dndt;
      }

      DsinitResult dsinitResult = new DsinitResult();

      dsinitResult.em = em;
      dsinitResult.argpm = argpm; 
      dsinitResult.inclm = inclm;
      dsinitResult.mm = mm;
      dsinitResult.nm =  nm;
      dsinitResult.nodem = nodem;

      dsinitResult.irez = irez;
      dsinitResult.atime = atime;

      dsinitResult.d2201 = d2201;
      dsinitResult.d2211 = d2211;
      dsinitResult.d3210 = d3210;
      dsinitResult.d3222 = d3222;
      dsinitResult.d4410 = d4410;

      dsinitResult.d4422 = d4422;
      dsinitResult.d5220 = d5220;
      dsinitResult.d5232 = d5232;
      dsinitResult.d5421 = d5421;
      dsinitResult.d5433 = d5433;

      dsinitResult.dedt = dedt;
      dsinitResult.didt = didt;
      dsinitResult.dmdt = dmdt;
      dsinitResult.dndt = dndt;
      dsinitResult.dnodt = dnodt;
      dsinitResult.domdt = domdt;

      dsinitResult.del1 = del1;
      dsinitResult.del2 = del2;
      dsinitResult.del3 = del3;

      dsinitResult.xfact = xfact;
      dsinitResult.xlamo = xlamo;
      dsinitResult.xli = xli;
      dsinitResult.xni = xni;

      return dsinitResult;

    }

  }

}


