using System;

namespace Satellite_cs{

  public class Dpper{
    
   
    
    public Dpper(){

    }

    /* -----------------------------------------------------------------------------
 *
 *                           procedure dpper
 *
 *  this procedure provides deep space long period periodic contributions
 *    to the mean elements.  by design, these periodics are zero at epoch.
 *    this used to be dscom which included initialization, but it's really a
 *    recurring function.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    e3          -
 *    ee2         -
 *    peo         -
 *    pgho        -
 *    pho         -
 *    pinco       -
 *    plo         -
 *    se2 , se3 , sgh2, sgh3, sgh4, sh2, sh3, si2, si3, sl2, sl3, sl4 -
 *    t           -
 *    xh2, xh3, xi2, xi3, xl2, xl3, xl4 -
 *    zmol        -
 *    zmos        -
 *    ep          - eccentricity                           0.0 - 1.0
 *    inclo       - inclination - needed for lyddane modification
 *    nodep       - right ascension of ascending node
 *    argpp       - argument of perigee
 *    mp          - mean anomaly
 *
 *  outputs       :
 *    ep          - eccentricity                           0.0 - 1.0
 *    inclp       - inclination
 *    nodep        - right ascension of ascending node
 *    argpp       - argument of perigee
 *    mp          - mean anomaly
 *
 *  locals        :
 *    alfdp       -
 *    betdp       -
 *    cosip  , sinip  , cosop  , sinop  ,
 *    dalf        -
 *    dbet        -
 *    dls         -
 *    f2, f3      -
 *    pe          -
 *    pgh         -
 *    ph          -
 *    pinc        -
 *    pl          -
 *    sel   , ses   , sghl  , sghs  , shl   , shs   , sil   , sinzf , sis   ,
 *    sll   , sls
 *    xls         -
 *    xnoh        -
 *    zf          -
 *    zm          -
 *
 *  coupling      :
 *    none.
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report #3 1980
 *    hoots, norad spacetrack report #6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/

    public DpperResult dpper(Satrec satrec, DpperOptions dpperOptions){

      Globals globals = new Globals();
      double pi = globals.pi;
      double twoPi = globals.twoPi;

      double e3 = satrec.e3;
      double ee2 = satrec.ee2;
      double peo = satrec.peo;
      double pgho = satrec.pgho;
      double pho = satrec.pho;
      double pinco = satrec.pinco;
      double plo = satrec.plo;
      double se2 = satrec.se2;
      double se3 = satrec.se3;
      double sgh2 = satrec.sgh2;
      double sgh3 = satrec.sgh3;
      double sgh4 = satrec.sgh4;
      double sh2 = satrec.sh2;
      double sh3 = satrec.sh3;
      double si2 = satrec.si2;
      double si3 = satrec.si3;
      double sl2 = satrec.sl2;
      double sl3 = satrec.sl3;
      double sl4 = satrec.sl4;
      double t = satrec.t;
      double xgh2 = satrec.xgh2;
      double xgh3 = satrec.xgh3;
      double xgh4 = satrec.xgh4;
      double xh2 = satrec.xh2;
      double xh3 = satrec.xh3;
      double xi2 = satrec.xi2;
      double xi3 = satrec.xi3;
      double xl2 = satrec.xl2;
      double xl3 = satrec.xl3;
      double xl4 = satrec.xl4;
      double zmol = satrec.zmol;
      double zmos = satrec.zmos;

      double init = dpperOptions.init;
      double opsmode = dpperOptions.opsmode;

      double ep = dpperOptions.ep;
      double inclp = dpperOptions.inclp;
      double nodep = dpperOptions.nodep;
      double argpp = dpperOptions.argpp;
      double mp = dpperOptions.mp;
      
      // Copy satellite attributes into local variables for convenience
      // and symmetry in writing formulae.

      double alfdp = 0.0;
      double betdp = 0.0;
      double cosip = 0.0;
      double sinip = 0.0;
      double cosop = 0.0;
      double sinop = 0.0;
      double dalf = 0.0;
      double dbet = 0.0;
      double dls = 0.0;
      double f2 = 0.0;
      double f3 = 0.0;
      double pe = 0.0;
      double pgh = 0.0;
      double ph = 0.0;
      double pinc = 0.0;
      double pl = 0.0;
      double sinzf = 0.0;
      double xls = 0.0;
      double xnoh = 0.0;
      double zf = 0.0;
      double zm = 0.0;

      //  ---------------------- constants -----------------------------
      double zns = 1.19459e-5;
      double zes = 0.01675;
      double znl = 1.5835218e-4;
      double zel = 0.05490;

      //  --------------- calculate time varying periodics -----------
      zm = zmos + (zns * t);

      // be sure that the initial call has time set to zero
      if (init == 'y') {
        zm = zmos;
      }
      zf = zm + (2.0 * zes * Math.Sin(zm));
      sinzf = Math.Sin(zf);
      f2 = (0.5 * sinzf * sinzf) - 0.25;
      f3 = -0.5 * sinzf * Math.Cos(zf);

      double ses = (se2 * f2) + (se3 * f3);
      double sis = (si2 * f2) + (si3 * f3);
      double sls = (sl2 * f2) + (sl3 * f3) + (sl4 * sinzf);
      double sghs = (sgh2 * f2) + (sgh3 * f3) + (sgh4 * sinzf);
      double shs = (sh2 * f2) + (sh3 * f3);

      zm = zmol + (znl * t);
      if (init == 'y') {
        zm = zmol;
      }

      zf = zm + (2.0 * zel * Math.Sin(zm));
      sinzf = Math.Sin(zf);
      f2 = (0.5 * sinzf * sinzf) - 0.25;
      f3 = -0.5 * sinzf * Math.Cos(zf);

      double sel = (ee2 * f2) + (e3 * f3);
      double sil = (xi2 * f2) + (xi3 * f3);
      double sll = (xl2 * f2) + (xl3 * f3) + (xl4 * sinzf);
      double sghl = (xgh2 * f2) + (xgh3 * f3) + (xgh4 * sinzf);
      double shll = (xh2 * f2) + (xh3 * f3);

      pe = ses + sel;
      pinc = sis + sil;
      pl = sls + sll;
      pgh = sghs + sghl;
      ph = shs + shll;

      if (init == 'n') {
        pe -= peo;
        pinc -= pinco;
        pl -= plo;
        pgh -= pgho;
        ph -= pho;
        inclp += pinc;
        ep += pe;
        sinip = Math.Sin(inclp);
        cosip = Math.Cos(inclp);

        /* ----------------- apply periodics directly ------------ */
        // sgp4fix for lyddane choice
        // strn3 used original inclination - this is technically feasible
        // gsfc used perturbed inclination - also technically feasible
        // probably best to readjust the 0.2 limit value and limit discontinuity
        // 0.2 rad = 11.45916 deg
        // use next line for original strn3 approach and original inclination
        // if (inclo >= 0.2)
        // use next line for gsfc version and perturbed inclination

        if (inclp >= 0.2) {
          ph /= sinip;
          pgh -= cosip * ph;
          argpp += pgh;
          nodep += ph;
          mp += pl;
        } else {
          //  ---- apply periodics with lyddane modification ----
          sinop = Math.Sin(nodep);
          cosop = Math.Cos(nodep);
          alfdp = sinip * sinop;
          betdp = sinip * cosop;
          dalf = (ph * cosop) + (pinc * cosip * sinop);
          dbet = (-ph * sinop) + (pinc * cosip * cosop);
          alfdp += dalf;
          betdp += dbet;
          nodep %= twoPi;

          //  sgp4fix for afspc written intrinsic functions
          //  nodep used without a trigonometric function ahead
          if (nodep < 0.0 && opsmode == 'a') {
            nodep += twoPi;
          }
          xls = mp + argpp + (cosip * nodep);
          dls = (pl + pgh) - (pinc * nodep * sinip);
          xls += dls;
          xnoh = nodep;
          nodep = Math.Atan2(alfdp, betdp); 

          //  sgp4fix for afspc written intrinsic functions
          //  nodep used without a trigonometric function ahead
          if (nodep < 0.0 && opsmode == 'a') {
            nodep += twoPi;
          }
          if (Math.Abs(xnoh - nodep) > pi) {
            if (nodep < xnoh) {
              nodep += twoPi;
            } else {
              nodep -= twoPi;
            }
          }
          mp += pl;
          argpp = xls - mp - (cosip * nodep);
        }
      }


      DpperResult dpperResult = new DpperResult();

      dpperResult.ep = ep;
      dpperResult.inclp = inclp;
      dpperResult.nodep = nodep;
      dpperResult.argpp = argpp;
      dpperResult.mp = mp;

      return dpperResult;
    }

  }

}