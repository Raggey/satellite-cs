using System;

namespace Satellite_cs{

  public class Dspace {


    public Dspace(){
 
    }

/*-----------------------------------------------------------------------------
 *
 *                           procedure dspace
 *
 *  this procedure provides deep space contributions to mean elements for
 *    perturbing third body.  these effects have been averaged over one
 *    revolution of the sun and moon.  for earth resonance effects, the
 *    effects have been averaged over no revolutions of the satellite.
 *    (mean motion)
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    d2201, d2211, d3210, d3222, d4410, d4422, d5220, d5232, d5421, d5433 -
 *    dedt        -
 *    del1, del2, del3  -
 *    didt        -
 *    dmdt        -
 *    dnodt       -
 *    domdt       -
 *    irez        - flag for resonance           0-none, 1-one day, 2-half day
 *    argpo       - argument of perigee
 *    argpdot     - argument of perigee dot (rate)
 *    t           - time
 *    tc          -
 *    gsto        - gst
 *    xfact       -
 *    xlamo       -
 *    no          - mean motion
 *    atime       -
 *    em          - eccentricity
 *    ft          -
 *    argpm       - argument of perigee
 *    inclm       - inclination
 *    xli         -
 *    mm          - mean anomaly
 *    xni         - mean motion
 *    nodem       - right ascension of ascending node
 *
 *  outputs       :
 *    atime       -
 *    em          - eccentricity
 *    argpm       - argument of perigee
 *    inclm       - inclination
 *    xli         -
 *    mm          - mean anomaly
 *    xni         -
 *    nodem       - right ascension of ascending node
 *    dndt        -
 *    nm          - mean motion
 *
 *  locals        :
 *    delt        -
 *    ft          -
 *    theta       -
 *    x2li        -
 *    x2omi       -
 *    xl          -
 *    xldot       -
 *    xnddt       -
 *    xndt        -
 *    xomi        -
 *
 *  coupling      :
 *    none        -
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report #3 1980
 *    hoots, norad spacetrack report #6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/
    public DspaceResults dspace(DspaceOptions dspaceOptions){

      Globals globals = new Globals();
      double twoPi = globals.twoPi;

      double irez = dspaceOptions.irez;
      double d2201 = dspaceOptions.d2201;
      double d2211 = dspaceOptions.d2211;
      double d3210 = dspaceOptions.d3210;
      double d3222 = dspaceOptions.d3222;
      double d4410 = dspaceOptions.d4410;
      double d4422 = dspaceOptions.d4422;
      double d5220 = dspaceOptions.d5220;
      double d5232 = dspaceOptions.d5232;
      double d5421 = dspaceOptions.d5421;
      double d5433 = dspaceOptions.d5433;
      double dedt = dspaceOptions.dedt;
      double del1 = dspaceOptions.del1;
      double del2 = dspaceOptions.del2;
      double del3 = dspaceOptions.del3;
      double didt = dspaceOptions.didt;
      double dmdt = dspaceOptions.dmdt;
      double dnodt = dspaceOptions.dnodt;
      double domdt = dspaceOptions.domdt;
      double argpo = dspaceOptions.argpo;
      double argpdot = dspaceOptions.argpdot;
      double t = dspaceOptions.t;
      double tc = dspaceOptions.tc;
      double gsto = dspaceOptions.gsto;
      double xfact = dspaceOptions.xfact;
      double xlamo = dspaceOptions.xlamo;
      double no = dspaceOptions.no;

      double atime = dspaceOptions.atime;
      double em = dspaceOptions.em;
      double argpm = dspaceOptions.argpm;
      double inclm = dspaceOptions.inclm;
      double xli = dspaceOptions.xli;
      double mm = dspaceOptions.mm;
      double xni = dspaceOptions.xni;
      double nodem = dspaceOptions.nodem;
      double nm = dspaceOptions.nm;

      double fasx2 = 0.13130908;
      double fasx4 = 2.8843198;
      double fasx6 = 0.37448087;
      double g22 = 5.7686396;
      double g32 = 0.95240898;
      double g44 = 1.8014998;
      double g52 = 1.0508330;
      double g54 = 4.4108898;
      double rptim = 4.37526908801129966e-3; // equates to 7.29211514668855e-5 rad/sec
      double stepp = 720.0;
      double stepn = -720.0;
      double step2 = 259200.0;

      double delt = 0.0;
      double x2li = 0.0;
      double x2omi = 0.0;
      double xl = 0.0;
      double xldot = 0.0;
      double xnddt = 0.0;
      double xndt = 0.0;
      double xomi = 0.0;
      double dndt = 0.0;
      double ft = 0.0;

      //  ----------- calculate deep space resonance effects -----------
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
      //   inclm = -inclm;
      //   argpm = argpm - pi;
      //   nodem = nodem + pi;
      // }

      /* - update resonances : numerical (euler-maclaurin) integration - */
      /* ------------------------- epoch restart ----------------------  */
      //   sgp4fix for propagator problems
      //   the following integration works for negative time steps and periods
      //   the specific changes are unknown because the original code was so convoluted

      // sgp4fix take out atime = 0.0 and fix for faster operation


      if (irez != 0) {
        //  sgp4fix streamline check
        if (atime == 0.0 || t * atime <= 0.0 || Math.Abs(t) < Math.Abs(atime)) {
          atime = 0.0;
          xni = no;
          xli = xlamo;
        }

        // sgp4fix move check outside loop
        if (t > 0.0) {
          delt = stepp;
        } else {
          delt = stepn;
        }

        double iretn = 381; // added for do loop
        while (iretn == 381) {
          //  ------------------- dot terms calculated -------------
          //  ----------- near - synchronous resonance terms -------
          if (irez != 2) {
            xndt = (del1 * Math.Sin(xli - fasx2))
              + (del2 * Math.Sin(2.0 * (xli - fasx4)))
              + (del3 * Math.Sin(3.0 * (xli - fasx6)));
            xldot = xni + xfact;
            xnddt = (del1 * Math.Cos(xli - fasx2))
              + (2.0 * del2 * Math.Cos(2.0 * (xli - fasx4)))
              + (3.0 * del3 * Math.Cos(3.0 * (xli - fasx6)));
            xnddt *= xldot;
          } else {
            // --------- near - half-day resonance terms --------
            xomi = argpo + (argpdot * atime);
            x2omi = xomi + xomi;
            x2li = xli + xli;
            xndt = (d2201 * Math.Sin((x2omi + xli) - g22))
              + (d2211 * Math.Sin(xli - g22))
              + (d3210 * Math.Sin((xomi + xli) - g32))
              + (d3222 * Math.Sin((-xomi + xli) - g32))
              + (d4410 * Math.Sin((x2omi + x2li) - g44))
              + (d4422 * Math.Sin(x2li - g44))
              + (d5220 * Math.Sin((xomi + xli) - g52))
              + (d5232 * Math.Sin((-xomi + xli) - g52))
              + (d5421 * Math.Sin((xomi + x2li) - g54))
              + (d5433 * Math.Sin((-xomi + x2li) - g54));
            xldot = xni + xfact;
            xnddt = (d2201 * Math.Cos((x2omi + xli) - g22))
              + (d2211 * Math.Cos(xli - g22))
              + (d3210 * Math.Cos((xomi + xli) - g32))
              + (d3222 * Math.Cos((-xomi + xli) - g32))
              + (d5220 * Math.Cos((xomi + xli) - g52))
              + (d5232 * Math.Cos((-xomi + xli) - g52))
              + (2.0 * d4410 * Math.Cos((x2omi + x2li) - g44))
              + (d4422 * Math.Cos(x2li - g44))
              + (d5421 * Math.Cos((xomi + x2li) - g54))
              + (d5433 * Math.Cos((-xomi + x2li) - g54));
            xnddt *= xldot;
          }

          //  ----------------------- integrator -------------------
          //  sgp4fix move end checks to end of routine
          if (Math.Abs(t - atime) >= stepp) {
            iretn = 381;
          } else {
            ft = t - atime;
            iretn = 0;
          }

          if (iretn == 381) {
            xli += (xldot * delt) + (xndt * step2);
            xni += (xndt * delt) + (xnddt * step2);
            atime += delt;
          }
        }

        nm = xni + (xndt * ft) + (xnddt * ft * ft * 0.5);
        xl = xli + (xldot * ft) + (xndt * ft * ft * 0.5);
        if (irez != 1) {
          mm = (xl - (2.0 * nodem)) + (2.0 * theta);
          dndt = nm - no;
        } else {
          mm = (xl - nodem - argpm) + theta;
          dndt = nm - no;
        }
        nm = no + dndt;
      }


      DspaceResults dspaceResults = new DspaceResults();


      dspaceResults.atime = atime;
      dspaceResults.em = em;
      dspaceResults.argpm = argpm;
      dspaceResults.inclm = inclm;
      dspaceResults.xli = xli;
      dspaceResults.mm = mm;
      dspaceResults.xni = xni;
      dspaceResults.nodem = nodem;
      dspaceResults.dndt = dndt;
      dspaceResults.nm = nm;

      return dspaceResults;
    }


  }

 
}