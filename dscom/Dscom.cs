using System;

namespace Satellite_cs{

  public class Dscom{
    
   
    
    public Dscom(){

    }

    /*-----------------------------------------------------------------------------
 *
 *                           procedure dscom
 *
 *  this procedure provides deep space common items used by both the secular
 *    and periodics subroutines.  input is provided as shown. this routine
 *    used to be called dpper, but the functions inside weren't well organized.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    epoch       -
 *    ep          - eccentricity
 *    argpp       - argument of perigee
 *    tc          -
 *    inclp       - inclination
 *    nodep       - right ascension of ascending node
 *    np          - mean motion
 *
 *  outputs       :
 *    sinim  , cosim  , sinomm , cosomm , snodm  , cnodm
 *    day         -
 *    e3          -
 *    ee2         -
 *    em          - eccentricity
 *    emsq        - eccentricity squared
 *    gam         -
 *    peo         -
 *    pgho        -
 *    pho         -
 *    pinco       -
 *    plo         -
 *    rtemsq      -
 *    se2, se3         -
 *    sgh2, sgh3, sgh4        -
 *    sh2, sh3, si2, si3, sl2, sl3, sl4         -
 *    s1, s2, s3, s4, s5, s6, s7          -
 *    ss1, ss2, ss3, ss4, ss5, ss6, ss7, sz1, sz2, sz3         -
 *    sz11, sz12, sz13, sz21, sz22, sz23, sz31, sz32, sz33        -
 *    xgh2, xgh3, xgh4, xh2, xh3, xi2, xi3, xl2, xl3, xl4         -
 *    nm          - mean motion
 *    z1, z2, z3, z11, z12, z13, z21, z22, z23, z31, z32, z33         -
 *    zmol        -
 *    zmos        -
 *
 *  locals        :
 *    a1, a2, a3, a4, a5, a6, a7, a8, a9, a10         -
 *    betasq      -
 *    cc          -
 *    ctem, stem        -
 *    x1, x2, x3, x4, x5, x6, x7, x8          -
 *    xnodce      -
 *    xnoi        -
 *    zcosg  , zsing  , zcosgl , zsingl , zcosh  , zsinh  , zcoshl , zsinhl ,
 *    zcosi  , zsini  , zcosil , zsinil ,
 *    zx          -
 *    zy          -
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

    public DscomResult dscom(DscomOptions dscomOptions){

      Globals globals = new Globals();
      double twoPi = globals.twoPi;

      double epoch = dscomOptions.epoch;
      double ep = dscomOptions.ep;
      double argpp = dscomOptions.argpp;
      double tc = dscomOptions.tc;
      double inclp = dscomOptions.inclp;
      double nodep = dscomOptions.nodep;
      double np = dscomOptions.np;

      double a1 = 0.0;
      double a2 = 0.0;
      double a3 = 0.0;
      double a4 = 0.0;
      double a5 = 0.0;
      double a6 = 0.0;
      double a7 = 0.0;
      double a8 = 0.0;
      double a9 = 0.0;
      double a10 = 0.0;
      double cc = 0.0;
      double x1 = 0.0;
      double x2 = 0.0;
      double x3 = 0.0;
      double x4 = 0.0;
      double x5 = 0.0;
      double x6 = 0.0;
      double x7 = 0.0;
      double x8 = 0.0;
      double zcosg = 0.0;
      double zsing = 0.0;
      double zcosh = 0.0;
      double zsinh = 0.0;
      double zcosi = 0.0;
      double zsini = 0.0;

      double ss1 = 0.0;
      double ss2 = 0.0;
      double ss3 = 0.0;
      double ss4 = 0.0;
      double ss5 = 0.0;
      double ss6 = 0.0;
      double ss7 = 0.0;
      double sz1 = 0.0;
      double sz2 = 0.0;
      double sz3 = 0.0;
      double sz11 = 0.0;
      double sz12 = 0.0;
      double sz13 = 0.0;
      double sz21 = 0.0;
      double sz22 = 0.0;
      double sz23 = 0.0;
      double sz31 = 0.0;
      double sz32 = 0.0;
      double sz33 = 0.0;
      double s1 = 0.0;
      double s2 = 0.0;
      double s3 = 0.0;
      double s4 = 0.0;
      double s5 = 0.0;
      double s6 = 0.0;
      double s7 = 0.0;
      double z1 = 0.0;
      double z2 = 0.0;
      double z3 = 0.0;
      double z11 = 0.0;
      double z12 = 0.0;
      double z13 = 0.0;
      double z21 = 0.0;
      double z22 = 0.0;
      double z23 = 0.0;
      double z31 = 0.0;
      double z32 = 0.0;
      double z33 = 0.0;

      // -------------------------- ants -------------------------
      double zes = 0.01675;
      double zel = 0.05490;
      double c1ss = 2.9864797e-6;
      double c1l = 4.7968065e-7;
      double zsinis = 0.39785416;
      double zcosis = 0.91744867;
      double zcosgs = 0.1945905;
      double zsings = -0.98088458;

    //  --------------------- local variables ------------------------
      double nm = np;
      double em = ep;
      double snodm = Math.Sin(nodep);
      double cnodm = Math.Cos(nodep);
      double sinomm = Math.Sin(argpp);
      double cosomm = Math.Cos(argpp);
      double sinim = Math.Sin(inclp);
      double cosim = Math.Cos(inclp);
      double emsq = em * em;
      double betasq = 1.0 - emsq;
      double rtemsq = Math.Sqrt(betasq);

      //  ----------------- initialize lunar solar terms ---------------
      double peo = 0.0;
      double pinco = 0.0;
      double plo = 0.0;
      double pgho = 0.0;
      double pho = 0.0;
      double day = epoch + 18261.5 + (tc / 1440.0);
      double xnodce = (4.5236020 - (9.2422029e-4 * day)) % twoPi;
      double stem = Math.Sin(xnodce);
      double ctem = Math.Cos(xnodce);
      double zcosil = 0.91375164 - (0.03568096 * ctem);
      double zsinil = Math.Sqrt(1.0 - (zcosil * zcosil));
      double zsinhl = (0.089683511 * stem) / zsinil;
      double zcoshl = Math.Sqrt(1.0 - (zsinhl * zsinhl));
      double gam = 5.8351514 + (0.0019443680 * day);
      double zx = (0.39785416 * stem) / zsinil;
      double zy = (zcoshl * ctem) + (0.91744867 * zsinhl * stem);
      zx = Math.Atan2(zy, zx); // CS Math.Atan2 args are reversed compared to JS
      zx += gam - xnodce;
      double zcosgl = Math.Cos(zx);
      double zsingl = Math.Sin(zx);

      //  ------------------------- do solar terms ---------------------
      zcosg = zcosgs;
      zsing = zsings;
      zcosi = zcosis;
      zsini = zsinis;
      zcosh = cnodm;
      zsinh = snodm;
      cc = c1ss;
      double xnoi = 1.0 / nm;

      double lsflg = 0;

      while (lsflg < 2) {
        lsflg += 1;
        a1 = (zcosg * zcosh) + (zsing * zcosi * zsinh);
        a3 = (-zsing * zcosh) + (zcosg * zcosi * zsinh);
        a7 = (-zcosg * zsinh) + (zsing * zcosi * zcosh);
        a8 = zsing * zsini;
        a9 = (zsing * zsinh) + (zcosg * zcosi * zcosh);
        a10 = zcosg * zsini;
        a2 = (cosim * a7) + (sinim * a8);
        a4 = (cosim * a9) + (sinim * a10);
        a5 = (-sinim * a7) + (cosim * a8);
        a6 = (-sinim * a9) + (cosim * a10);

        x1 = (a1 * cosomm) + (a2 * sinomm);
        x2 = (a3 * cosomm) + (a4 * sinomm);
        x3 = (-a1 * sinomm) + (a2 * cosomm);
        x4 = (-a3 * sinomm) + (a4 * cosomm);
        x5 = a5 * sinomm;
        x6 = a6 * sinomm;
        x7 = a5 * cosomm;
        x8 = a6 * cosomm;

        z31 = (12.0 * x1 * x1) - (3.0 * x3 * x3);
        z32 = (24.0 * x1 * x2) - (6.0 * x3 * x4);
        z33 = (12.0 * x2 * x2) - (3.0 * x4 * x4);

        z1 = (3.0 * ((a1 * a1) + (a2 * a2))) + (z31 * emsq);
        z2 = (6.0 * ((a1 * a3) + (a2 * a4))) + (z32 * emsq);
        z3 = (3.0 * ((a3 * a3) + (a4 * a4))) + (z33 * emsq);

        z11 = (-6.0 * a1 * a5)
          + (emsq * ((-24.0 * x1 * x7) - (6.0 * x3 * x5)));
        z12 = (-6.0 * ((a1 * a6) + (a3 * a5)))
          + (emsq * ((-24.0 * ((x2 * x7) + (x1 * x8))) + (-6.0 * ((x3 * x6) + (x4 * x5)))));

        z13 = (-6.0 * a3 * a6)
          + (emsq * ((-24.0 * x2 * x8) - (6.0 * x4 * x6)));

        z21 = (6.0 * a2 * a5)
          + (emsq * ((24.0 * x1 * x5) - (6.0 * x3 * x7)));
        z22 = (6.0 * ((a4 * a5) + (a2 * a6)))
          + (emsq * ((24.0 * ((x2 * x5) + (x1 * x6))) - (6.0 * ((x4 * x7) + (x3 * x8)))));
        z23 = (6.0 * a4 * a6)
          + (emsq * ((24.0 * x2 * x6) - (6.0 * x4 * x8)));

        z1 = z1 + z1 + (betasq * z31);
        z2 = z2 + z2 + (betasq * z32);
        z3 = z3 + z3 + (betasq * z33);
        s3 = cc * xnoi;
        s2 = (-0.5 * s3) / rtemsq;
        s4 = s3 * rtemsq;
        s1 = -15.0 * em * s4;
        s5 = (x1 * x3) + (x2 * x4);
        s6 = (x2 * x3) + (x1 * x4);
        s7 = (x2 * x4) - (x1 * x3);

        //  ----------------------- do lunar terms -------------------
        if (lsflg == 1) {
          ss1 = s1;
          ss2 = s2;
          ss3 = s3;
          ss4 = s4;
          ss5 = s5;
          ss6 = s6;
          ss7 = s7;
          sz1 = z1;
          sz2 = z2;
          sz3 = z3;
          sz11 = z11;
          sz12 = z12;
          sz13 = z13;
          sz21 = z21;
          sz22 = z22;
          sz23 = z23;
          sz31 = z31;
          sz32 = z32;
          sz33 = z33;
          zcosg = zcosgl;
          zsing = zsingl;
          zcosi = zcosil;
          zsini = zsinil;
          zcosh = (zcoshl * cnodm) + (zsinhl * snodm);
          zsinh = (snodm * zcoshl) - (cnodm * zsinhl);
          cc = c1l;
        }
      }

      double zmol = (4.7199672 + ((0.22997150 * day) - gam)) % twoPi;
      double zmos = (6.2565837 + (0.017201977 * day)) % twoPi;

      //  ------------------------ do solar terms ----------------------
      double se2 = 2.0 * ss1 * ss6;
      double se3 = 2.0 * ss1 * ss7;
      double si2 = 2.0 * ss2 * sz12;
      double si3 = 2.0 * ss2 * (sz13 - sz11);
      double sl2 = -2.0 * ss3 * sz2;
      double sl3 = -2.0 * ss3 * (sz3 - sz1);
      double sl4 = -2.0 * ss3 * (-21.0 - (9.0 * emsq)) * zes;
      double sgh2 = 2.0 * ss4 * sz32;
      double sgh3 = 2.0 * ss4 * (sz33 - sz31);
      double sgh4 = -18.0 * ss4 * zes;
      double sh2 = -2.0 * ss2 * sz22;
      double sh3 = -2.0 * ss2 * (sz23 - sz21);

      //  ------------------------ do lunar terms ----------------------
      double ee2 = 2.0 * s1 * s6;
      double e3 = 2.0 * s1 * s7;
      double xi2 = 2.0 * s2 * z12;
      double xi3 = 2.0 * s2 * (z13 - z11);
      double xl2 = -2.0 * s3 * z2;
      double xl3 = -2.0 * s3 * (z3 - z1);
      double xl4 = -2.0 * s3 * (-21.0 - (9.0 * emsq)) * zel;
      double xgh2 = 2.0 * s4 * z32;
      double xgh3 = 2.0 * s4 * (z33 - z31);
      double xgh4 = -18.0 * s4 * zel;
      double xh2 = -2.0 * s2 * z22;
      double xh3 = -2.0 * s2 * (z23 - z21);

      DscomResult dscomResult = new DscomResult();

      dscomResult.snodm = snodm;
      dscomResult.cnodm = cnodm;
      dscomResult.sinim = sinim;
      dscomResult.cosim = cosim;
      dscomResult.sinomn = sinomm;

      dscomResult.cosomm = cosomm;
      dscomResult.day = day;
      dscomResult.e3 = e3;
      dscomResult.ee2 = ee2;
      dscomResult.em = em;

      dscomResult.emsq = emsq;
      dscomResult.gam = gam;
      dscomResult.peo = peo;
      dscomResult.pgho = pgho;
      dscomResult.pho = pho;

      dscomResult.pinco = pinco;
      dscomResult.plo = plo;
      dscomResult.rtemsq = rtemsq;
      dscomResult.se2 = se2;
      dscomResult.se3 = se3;

      dscomResult.sgh2 = sgh2;
      dscomResult.sgh3 = sgh3;
      dscomResult.sgh4 = sgh4;
      dscomResult.sh2 = sh2;
      dscomResult.sh3 = sh3;

      dscomResult.si2 = si2;
      dscomResult.si2 = si3;
      dscomResult.sl2 = sl2;
      dscomResult.sl3 = sl3;
      dscomResult.sl4 = sl4;

      dscomResult.s1 = s1;
      dscomResult.s2 = s2;
      dscomResult.s3 = s3;
      dscomResult.s4 = s4;
      dscomResult.s5 = s5;

      dscomResult.s6 = s6;
      dscomResult.s7 = s7;
      dscomResult.ss1 = ss1;
      dscomResult.ss2 = ss2;
      dscomResult.ss3 = ss3;

      dscomResult.ss4 = ss4;
      dscomResult.ss5 = ss5;
      dscomResult.ss6 = ss6;
      dscomResult.ss7 = ss7;
      dscomResult.sz1 = sz1;

      dscomResult.sz2 = sz2;
      dscomResult.sz3 = sz3;
      dscomResult.sz11 = sz11;
      dscomResult.sz12 = sz12;
      dscomResult.sz13 = sz13;

      dscomResult.sz21 = sz21;
      dscomResult.sz22 = sz22;
      dscomResult.sz23 = sz23;
      dscomResult.sz31 = sz31;
      dscomResult.sz32 = sz32;

      dscomResult.sz33 = sz33;
      dscomResult.xgh2 = xgh2;
      dscomResult.xgh3 = xgh3;
      dscomResult.xgh4 = xgh4;
      dscomResult.xh2 =  xh2;

      dscomResult.xh3 = xh3;
      dscomResult.xi2 = xi2;
      dscomResult.xi3 = xi3;
      dscomResult.xl2 = xl2;
      dscomResult.xl3 = xl3;

      dscomResult.xl4 = xl4;
      dscomResult.nm = nm;
      dscomResult.z1 = z1;
      dscomResult.z2 = z2;
      dscomResult.z3 = z3;

      dscomResult.z11 = z11;
      dscomResult.z12 = z12;
      dscomResult.z13 = z13;
      dscomResult.z21 = z21;
      dscomResult.z21 = z22;

      dscomResult.z23 = z23;
      dscomResult.z31 = z31;
      dscomResult.z32 = z32;
      dscomResult.z33 = z33;
      dscomResult.zmol = zmol;

      dscomResult.zmos = zmos;

      return dscomResult;

    }

  }

}