using System;

namespace Satellite_cs{

  public class Sgp4init{
    
    public Sgp4init(){

    }

/*-----------------------------------------------------------------------------
 *
 *                             procedure sgp4init
 *
 *  this procedure initializes variables for sgp4.
 *
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *  author        : david vallado                  719-573-2600   28 jun 2005
 *
 *  inputs        :
 *    opsmode     - mode of operation afspc or improved 'a', 'i'
 *    satn        - satellite number
 *    bstar       - sgp4 type drag coefficient              kg/m2er
 *    ecco        - eccentricity
 *    epoch       - epoch time in days from jan 0, 1950. 0 hr
 *    argpo       - argument of perigee (output if ds)
 *    inclo       - inclination
 *    mo          - mean anomaly (output if ds)
 *    no          - mean motion
 *    nodeo       - right ascension of ascending node
 *
 *  outputs       :
 *    rec      - common values for subsequent calls
 *    return code - non-zero on error.
 *                   1 - mean elements, ecc >= 1.0 or ecc < -0.001 or a < 0.95 er
 *                   2 - mean motion less than 0.0
 *                   3 - pert elements, ecc < 0.0  or  ecc > 1.0
 *                   4 - semi-latus rectum < 0.0
 *                   5 - epoch elements are sub-orbital
 *                   6 - satellite has decayed
 *
 *  locals        :
 *    cnodm  , snodm  , cosim  , sinim  , cosomm , sinomm
 *    cc1sq  , cc2    , cc3
 *    coef   , coef1
 *    cosio4      -
 *    day         -
 *    dndt        -
 *    em          - eccentricity
 *    emsq        - eccentricity squared
 *    eeta        -
 *    etasq       -
 *    gam         -
 *    argpm       - argument of perigee
 *    nodem       -
 *    inclm       - inclination
 *    mm          - mean anomaly
 *    nm          - mean motion
 *    perige      - perigee
 *    pinvsq      -
 *    psisq       -
 *    qzms24      -
 *    rtemsq      -
 *    s1, s2, s3, s4, s5, s6, s7          -
 *    sfour       -
 *    ss1, ss2, ss3, ss4, ss5, ss6, ss7         -
 *    sz1, sz2, sz3
 *    sz11, sz12, sz13, sz21, sz22, sz23, sz31, sz32, sz33        -
 *    tc          -
 *    temp        -
 *    temp1, temp2, temp3       -
 *    tsi         -
 *    xpidot      -
 *    xhdot1      -
 *    z1, z2, z3          -
 *    z11, z12, z13, z21, z22, z23, z31, z32, z33         -
 *
 *  coupling      :
 *    getgravconst-
 *    initl       -
 *    dscom       -
 *    dpper       -
 *    dsinit      -
 *    sgp4        -
 *
 *  references    :
 *    hoots, roehrich, norad spacetrack report #3 1980
 *    hoots, norad spacetrack report #6 1986
 *    hoots, schumacher and glover 2004
 *    vallado, crawford, hujsak, kelso  2006
 ----------------------------------------------------------------------------*/
    public void sgp4init(Satrec satrec, Sgp4initOptions options){

      Globals globals = new Globals();

      double pi = globals.pi;
      double earthRadius = globals.earthRadius;
      double j2 = globals.j2;
      double j4 = globals.j4;
      double j3oj2 = globals.j3oj2;
      double x2o3 = globals.x2o3;

      char opsmode = options.opsmode;
      string satn = options.satn;
      double epoch = options.epoch;
      double xbstar = options.xbstar;
      double xecco = options.xecco;
      double xargpo  = options.xargpo;
      double xinclo = options.xinclo;
      double xmo = options.xmo;
      double xno = options.xno;
      double xnodeo = options.xnodeo;


      double cosim;
      double sinim;
      double cc1sq;
      double cc2;
      double cc3;
      double coef;
      double coef1;
      double cosio4;
      double em;
      double emsq;
      double eeta;
      double etasq;
      double argpm;
      double nodem;
      double inclm;
      double mm;
      double nm;
      double perige;
      double pinvsq;
      double psisq;
      double qzms24;
      double s1;
      double s2;
      double s3;
      double s4;
      double s5;
      double sfour;
      double ss1;
      double ss2;
      double ss3;
      double ss4;
      double ss5;
      double sz1;
      double sz3;
      double sz11;
      double sz13;
      double sz21;
      double sz23;
      double sz31;
      double sz33;
      double tc;
      double temp;
      double temp1;
      double temp2;
      double temp3;
      double tsi;
      double xpidot;
      double xhdot1;
      double z1;
      double z3;
      double z11;
      double z13;
      double z21;
      double z23;
      double z31;
      double z33;

      /* ------------------------ initialization --------------------- */
      // sgp4fix divisor for divide by zero check on inclination
      // the old check used 1.0 + Math.cos(pi-1.0e-9), but then compared it to
      // 1.5 e-12, so the threshold was changed to 1.5e-12 for consistency
      double temp4 = 1.5e-12;

      // ----------- set all near earth variables to zero ------------
      satrec.isimp = 0; satrec.method = 'n'; satrec.aycof = 0.0;
      satrec.con41 = 0.0; satrec.cc1 = 0.0; satrec.cc4 = 0.0;
      satrec.cc5 = 0.0; satrec.d2 = 0.0; satrec.d3 = 0.0;
      satrec.d4 = 0.0; satrec.delmo = 0.0; satrec.eta = 0.0;
      satrec.argpdot = 0.0; satrec.omgcof = 0.0; satrec.sinmao = 0.0;
      satrec.t = 0.0; satrec.t2cof = 0.0; satrec.t3cof = 0.0;
      satrec.t4cof = 0.0; satrec.t5cof = 0.0; satrec.x1mth2 = 0.0;
      satrec.x7thm1 = 0.0; satrec.mdot = 0.0; satrec.nodedot = 0.0;
      satrec.xlcof = 0.0; satrec.xmcof = 0.0; satrec.nodecf = 0.0;

    // ----------- set all deep space variables to zero ------------
      satrec.irez = 0; satrec.d2201 = 0.0; satrec.d2211 = 0.0;
      satrec.d3210 = 0.0; satrec.d3222 = 0.0; satrec.d4410 = 0.0;
      satrec.d4422 = 0.0; satrec.d5220 = 0.0; satrec.d5232 = 0.0;
      satrec.d5421 = 0.0; satrec.d5433 = 0.0; satrec.dedt = 0.0;
      satrec.del1 = 0.0; satrec.del2 = 0.0; satrec.del3 = 0.0;
      satrec.didt = 0.0; satrec.dmdt = 0.0; satrec.dnodt = 0.0;
      satrec.domdt = 0.0; satrec.e3 = 0.0; satrec.ee2 = 0.0;
      satrec.peo = 0.0; satrec.pgho = 0.0; satrec.pho = 0.0;
      satrec.pinco = 0.0; satrec.plo = 0.0; satrec.se2 = 0.0;
      satrec.se3 = 0.0; satrec.sgh2 = 0.0; satrec.sgh3 = 0.0;
      satrec.sgh4 = 0.0; satrec.sh2 = 0.0; satrec.sh3 = 0.0;
      satrec.si2 = 0.0; satrec.si3 = 0.0; satrec.sl2 = 0.0;
      satrec.sl3 = 0.0; satrec.sl4 = 0.0; satrec.gsto = 0.0;
      satrec.xfact = 0.0; satrec.xgh2 = 0.0; satrec.xgh3 = 0.0;
      satrec.xgh4 = 0.0; satrec.xh2 = 0.0; satrec.xh3 = 0.0;
      satrec.xi2 = 0.0; satrec.xi3 = 0.0; satrec.xl2 = 0.0;
      satrec.xl3 = 0.0; satrec.xl4 = 0.0; satrec.xlamo = 0.0;
      satrec.zmol = 0.0; satrec.zmos = 0.0; satrec.atime = 0.0;
      satrec.xli = 0.0; satrec.xni = 0.0;


      // sgp4fix - note the following variables are also passed directly via satrec.
      // it is possible to streamline the sgp4init call by deleting the "x"
      // variables, but the user would need to set the satrec.* values first. we
      // include the additional assignments in case twoline2rv is not used.

      satrec.bstar = xbstar;
      satrec.ecco = xecco;
      satrec.argpo = xargpo;
      satrec.inclo = xinclo;
      satrec.mo = xmo;
      satrec.no = xno;
      satrec.nodeo = xnodeo;

      //  sgp4fix add opsmode
      satrec.operationmode = opsmode;

       // ------------------------ earth constants -----------------------
      // sgp4fix identify constants and allow alternate values

      double ss = (78.0 / earthRadius) + 1.0;
      // sgp4fix use multiply for speed instead of pow
      double qzms2ttemp = (120.0 - 78.0) / earthRadius;
      double qzms2t = qzms2ttemp * qzms2ttemp * qzms2ttemp * qzms2ttemp;
      
      satrec.init = 'y';
      satrec.t = 0.0;

      // const initlOptions = {
      //   satn,
      //   ecco: satrec.ecco,

      //   epoch,
      //   inclo: satrec.inclo,
      //   no: satrec.no,

      //   method: satrec.method,
      //   opsmode: satrec.operationmode,
      // };

      // const initlResult = initl(initlOptions);
      
      InitlOptions initlOptions = new InitlOptions();
      initlOptions.satn = satn;
      initlOptions.ecco = satrec.ecco;
      initlOptions.epoch = epoch;
      initlOptions.inlco = satrec.inclo;
      initlOptions.no = satrec.no;
      initlOptions.method = satrec.method;
      initlOptions.opsmode = satrec.operationmode;

      Initl initl = new Initl();
      InitlResult initlResult = new InitlResult();

      initlResult = initl.initl(initlOptions); //TODO: Finish impl
      
      double ao = initlResult.ao;
      double con42 = initlResult.con42;
      double cosio = initlResult.cosio;
      double cosio2 = initlResult.cosio2;
      double eccsq = initlResult.eccsq;
      double omeosq = initlResult.omeosq;
      double posq = initlResult.posq;
      double rp = initlResult.rp;
      double rteosq = initlResult.rteosq;
      double sinio = initlResult.sinio;

      satrec.no = initlResult.no;
      satrec.con41 = initlResult.con41;
      satrec.gsto = initlResult.gsto;
      satrec.error = 0;







    // sgp4fix remove this check as it is unnecessary
    // the mrt check in sgp4 handles decaying satellite cases even if the starting
    // condition is below the surface of te earth
    // if (rp < 1.0)
    // {
    //   printf("// *** satn%d epoch elts sub-orbital ***\n", satn);
    //   satrec.error = 5;
    // }


    if (omeosq >= 0.0 || satrec.no >= 0.0) {
      satrec.isimp = 0;
      if (rp < (220.0 / earthRadius + 1.0)) {
        satrec.isimp = 1;
      }
      sfour = ss;
      qzms24 = qzms2t;
      perige = (rp - 1.0) * earthRadius;

      // - for perigees below 156 km, s and qoms2t are altered -
      if (perige < 156.0) {
        sfour = perige - 78.0;
        if (perige < 98.0) {
          sfour = 20.0;
        }

        // sgp4fix use multiply for speed instead of pow
        double qzms24temp = (120.0 - sfour) / earthRadius;
        qzms24 = qzms24temp * qzms24temp * qzms24temp * qzms24temp;
        sfour = (sfour / earthRadius) + 1.0;
      }
      pinvsq = 1.0 / posq;

      tsi = 1.0 / (ao - sfour);
      satrec.eta = ao * satrec.ecco * tsi;
      etasq = satrec.eta * satrec.eta;
      eeta = satrec.ecco * satrec.eta;
      psisq = Math.Abs(1.0 - etasq);
      coef = qzms24 *  Math.Pow(tsi,4.0) ;
      coef1 = coef /  (Math.Pow(psisq, 3.5));
      cc2 = coef1 * satrec.no * ((ao * (1.0 + (1.5 * etasq) + (eeta * (4.0 + etasq))))
        + (((0.375 * j2 * tsi) / psisq) * satrec.con41
          * (8.0 + (3.0 * etasq * (8.0 + etasq)))));
      satrec.cc1 = satrec.bstar * cc2;
      cc3 = 0.0;
      if (satrec.ecco > 1.0e-4) {
        cc3 = (-2.0 * coef * tsi * j3oj2 * satrec.no * sinio) / satrec.ecco;
      }
      satrec.x1mth2 = 1.0 - cosio2;
      satrec.cc4 = 2.0 * satrec.no * coef1 * ao * omeosq * (
        ((satrec.eta * (2.0 + (0.5 * etasq)))
          + (satrec.ecco * (0.5 + (2.0 * etasq))))
        - (((j2 * tsi) / (ao * psisq))
          * ((-3.0 * satrec.con41 * ((1.0 - (2.0 * eeta)) + (etasq * (1.5 - (0.5 * eeta)))))
            + (0.75 * satrec.x1mth2
              * ((2.0 * etasq) - (eeta * (1.0 + etasq)))
              * Math.Cos(2.0 * satrec.argpo))))
      );
      satrec.cc5 = 2.0 * coef1 * ao * omeosq * (1.0 + (2.75 * (etasq + eeta)) + (eeta * etasq));
      cosio4 = cosio2 * cosio2;
      temp1 = 1.5 * j2 * pinvsq * satrec.no;
      temp2 = 0.5 * temp1 * j2 * pinvsq;
      temp3 = -0.46875 * j4 * pinvsq * pinvsq * satrec.no;
      satrec.mdot = satrec.no + (0.5 * temp1 * rteosq * satrec.con41)
        + (0.0625 * temp2 * rteosq * ((13.0 - (78.0 * cosio2)) + (137.0 * cosio4)));
      satrec.argpdot = (-0.5 * temp1 * con42)
        + (0.0625 * temp2 * ((7.0 - (114.0 * cosio2)) + (395.0 * cosio4)))
        + (temp3 * ((3.0 - (36.0 * cosio2)) + (49.0 * cosio4)));
      xhdot1 = -temp1 * cosio;
      satrec.nodedot = xhdot1 + (((0.5 * temp2 * (4.0 - (19.0 * cosio2)))
        + (2.0 * temp3 * (3.0 - (7.0 * cosio2)))) * cosio);
      xpidot = satrec.argpdot + satrec.nodedot;
      satrec.omgcof = satrec.bstar * cc3 * Math.Cos(satrec.argpo);
      satrec.xmcof = 0.0;
      if (satrec.ecco > 1.0e-4) {
        satrec.xmcof = (-x2o3 * coef * satrec.bstar) / eeta;
      }
      satrec.nodecf = 3.5 * omeosq * xhdot1 * satrec.cc1;
      satrec.t2cof = 1.5 * satrec.cc1;

      // sgp4fix for divide by zero with xinco = 180 deg
      if (Math.Abs(cosio + 1.0) > 1.5e-12) {
        satrec.xlcof = (-0.25 * j3oj2 * sinio * (3.0 + (5.0 * cosio))) / (1.0 + cosio);
      } else {
        satrec.xlcof = (-0.25 * j3oj2 * sinio * (3.0 + (5.0 * cosio))) / temp4;
      }
      satrec.aycof = -0.5 * j3oj2 * sinio;

      // sgp4fix use multiply for speed instead of pow
      double delmotemp = 1.0 + (satrec.eta * Math.Cos(satrec.mo));
      satrec.delmo = delmotemp * delmotemp * delmotemp;
      satrec.sinmao = Math.Sin(satrec.mo);
      satrec.x7thm1 = (7.0 * cosio2) - 1.0;


       // --------------- deep space initialization -------------
      if ((2 * pi) / satrec.no >= 225.0) {
        satrec.method = 'd';
        satrec.isimp = 1;
        tc = 0.0;
        inclm = satrec.inclo;


        DscomOptions dscomOptions = new DscomOptions();
        dscomOptions.epoch = epoch;
        dscomOptions.ep = satrec.ecco;
        dscomOptions.argpp = satrec.argpo;
        dscomOptions.tc = tc;
        dscomOptions.inclp = satrec.inclo;
        dscomOptions.nodep = satrec.nodeo;
        
        dscomOptions.np = satrec.no;
        dscomOptions.e3 = satrec.e3;
        dscomOptions.ee2 = satrec.ee2;

        dscomOptions.peo = satrec.peo;
        dscomOptions.pgho = satrec.pgho;
        dscomOptions.pho = satrec.pho;
        dscomOptions.pinco = satrec.pinco;

        dscomOptions.plo = satrec.plo;
        dscomOptions.se2 = satrec.se2;
        dscomOptions.se3 = satrec.se3;

        dscomOptions.sgh2 = satrec.sgh2;
        dscomOptions.sgh3 = satrec.sgh3;
        dscomOptions.sgh4 = satrec.sgh4;

        dscomOptions.sh2 = satrec.sh2;
        dscomOptions.sh3 = satrec.sh3;
        dscomOptions.si2 = satrec.si2;
        dscomOptions.si3 = satrec.si3;

        dscomOptions.sl2 = satrec.sl2;
        dscomOptions.sl3 = satrec.sl3;
        dscomOptions.sl4 = satrec.sl4;
        
        dscomOptions.xgh2 = satrec.xgh2;
        dscomOptions.xgh3 = satrec.xgh3;
        dscomOptions.xgh4 = satrec.xgh4;
        dscomOptions.xh2 = satrec.xh2;
        
        dscomOptions.xh3 = satrec.xh3;
        dscomOptions.xi2 = satrec.xi2;
        dscomOptions.xi3 = satrec.xi3;
        dscomOptions.xl2 = satrec.xl2;
        
        dscomOptions.xl3 = satrec.xl3;
        dscomOptions.xl4 = satrec.xl4;

        dscomOptions.zmol = satrec.zmol;
        dscomOptions.zmos = satrec.zmos;

    

        Dscom dscom = new Dscom();
        DscomResult dscomResult = new DscomResult();

        dscomResult = dscom.dscom(dscomOptions); //TODO: Check 0.0 assignments are correct
        
        satrec.e3 = dscomResult.e3;
        satrec.ee2 = dscomResult.ee2;

        satrec.peo = dscomResult.peo;
        satrec.pgho = dscomResult.pgho;
        satrec.pho = dscomResult.pho;

        satrec.pinco = dscomResult.pinco;
        satrec.plo = dscomResult.plo;
        satrec.se2 = dscomResult.se2;
        satrec.se3 = dscomResult.se3;

        satrec.sgh2 = dscomResult.sgh2;
        satrec.sgh3 = dscomResult.sgh3;
        satrec.sgh4 = dscomResult.sgh4;
        satrec.sh2 = dscomResult.sh2;
        satrec.sh3 = dscomResult.sh3;

        satrec.si2 = dscomResult.si2;
        satrec.si3 = dscomResult.si3;
        satrec.sl2 = dscomResult.sl2;
        satrec.sl3 = dscomResult.sl3;
        satrec.sl4 = dscomResult.sl4;

        sinim = dscomResult.sinim;
        cosim = dscomResult.cosim;
        em = dscomResult.em;
        emsq = dscomResult.emsq;
        s1 = dscomResult.s1;
        s2 = dscomResult.s2;
        s3 = dscomResult.s3;
        s4 = dscomResult.s4;
        s5 = dscomResult.s5;
        ss1 = dscomResult.ss1;
        ss2 = dscomResult.ss2;
        ss3 = dscomResult.ss3;
        ss4 = dscomResult.ss4;
        ss5 = dscomResult.ss5;
        sz1 = dscomResult.sz1;
        sz3 = dscomResult.sz3;
        sz11 = dscomResult.sz11;
        sz13 = dscomResult.sz13;
        sz21 = dscomResult.sz21;
        sz23 = dscomResult.sz23;
        sz31 = dscomResult.sz31;
        sz33 = dscomResult.sz33;

        satrec.xgh2 = dscomResult.xgh2;
        satrec.xgh3 = dscomResult.xgh3;
        satrec.xgh4 = dscomResult.xgh4;
        satrec.xh2 = dscomResult.xh2;
        satrec.xh3 = dscomResult.xh3;
        satrec.xi2 = dscomResult.xi2;
        satrec.xi3 = dscomResult.xi3;
        satrec.xl2 = dscomResult.xl2;
        satrec.xl3 = dscomResult.xl3;
        satrec.xl4 = dscomResult.xl4;
        satrec.zmol = dscomResult.zmol;
        satrec.zmos = dscomResult.zmos;

        nm = dscomResult.nm;
        z1 = dscomResult.z1;
        z3 = dscomResult.z3;
        z11 = dscomResult.z11;
        z13 = dscomResult.z13;
        z21 = dscomResult.z21;
        z23 = dscomResult.z23;
        z31 = dscomResult.z31;
        z33 = dscomResult.z33;


        DpperOptions dpperOptions = new DpperOptions();
        dpperOptions.inclo = inclm;
        dpperOptions.init = satrec.init;
        dpperOptions.ep = satrec.ecco;
        dpperOptions.inclp = satrec.inclo;
        dpperOptions.nodep = satrec.nodeo;
        dpperOptions.argpp = satrec.argpo;
        dpperOptions.mp = satrec.mo;
        dpperOptions.opsmode = satrec.operationmode;


        Dpper dpper = new Dpper();
        DpperResult dpperResult = new DpperResult();

        dpperResult = dpper.dpper(satrec, dpperOptions); 

        satrec.ecco = dpperResult.ep;
        satrec.inclo = dpperResult.inclp;
        satrec.nodeo = dpperResult.nodep;
        satrec.argpo = dpperResult.argpp;
        satrec.mo = dpperResult.mp;

        argpm = 0.0;
        nodem = 0.0;
        mm = 0.0;



        DsinitOptions dsinitOptions = new DsinitOptions();

        dsinitOptions.cosim = cosim;
        dsinitOptions.emsq = emsq; 
        dsinitOptions.argpo = satrec.argpo; 
        dsinitOptions.s1 = s1; 
        dsinitOptions.s2 = s2; 
        dsinitOptions.s3 = s3;
        dsinitOptions.s4 = s4;
        dsinitOptions.s5 = s5;
        dsinitOptions.sinim = sinim;
        dsinitOptions.ss1 = ss1;
        dsinitOptions.ss2 = ss2;
        dsinitOptions.ss3 = ss3;
        dsinitOptions.ss4 = ss4;
        dsinitOptions.ss5 = ss5;
        dsinitOptions.sz1 = sz1;
        dsinitOptions.sz3 = sz3;
        dsinitOptions.sz11 = sz11;
        dsinitOptions.sz13 = sz13;
        dsinitOptions.sz21 = sz21;
        dsinitOptions.sz23 = sz23;
        dsinitOptions.sz31 = sz31;
        dsinitOptions.sz33 = sz33;
        dsinitOptions.t = satrec.t;

        dsinitOptions.tc = tc; 
        dsinitOptions.gsto = satrec.gsto;
        dsinitOptions.mo = satrec.mo;
        dsinitOptions.mdot = satrec.mdot;
        dsinitOptions.no = satrec.no;
        dsinitOptions.nodeo = satrec.nodeo;
        dsinitOptions.nodedot = satrec.nodedot;

        dsinitOptions.xpidot = xpidot;
        dsinitOptions.z1 = z1;
        dsinitOptions.z3 = z3;
        dsinitOptions.z11 = z11;
        dsinitOptions.z13 = z13;
        dsinitOptions.z21 = z21; 
        dsinitOptions.z23 = z23;
        dsinitOptions.z31 = z31;
        dsinitOptions.z33 = z33;
        dsinitOptions.ecco = satrec.ecco; 

        dsinitOptions.eccsq = eccsq; 
        dsinitOptions.em = em;
        dsinitOptions.argpm = argpm;
        dsinitOptions.inclm = inclm;
        dsinitOptions.mm = mm;
        dsinitOptions.nm = nm;
        dsinitOptions.nodem = nodem;

        dsinitOptions.irez = satrec.irez;
        dsinitOptions.atime = satrec.atime;
        dsinitOptions.d2201 = satrec.d2201;
        dsinitOptions.d2211 = satrec.d2211;
        dsinitOptions.d3210 = satrec.d3210;
        dsinitOptions.d3222 = satrec.d3222;
        dsinitOptions.d4410 = satrec.d4410;
        dsinitOptions.d4422 = satrec.d4422;
        dsinitOptions.d5220 = satrec.d5220;
        dsinitOptions.d5232 = satrec.d5232;
        dsinitOptions.d5421 = satrec.d5421;
        dsinitOptions.d5433 = satrec.d5433;
        dsinitOptions.dedt = satrec.dedt;
        dsinitOptions.didt = satrec.didt;
        dsinitOptions.dmdt = satrec.dmdt;
        dsinitOptions.dnodt = satrec.dnodt;
        dsinitOptions.domdt = satrec.domdt;
        dsinitOptions.del1 = satrec.del1;
        dsinitOptions.del2 = satrec.del2;
        dsinitOptions.del3 = satrec.del3;
        dsinitOptions.xfact = satrec.xfact;
        dsinitOptions.xlamo = satrec.xlamo;
        dsinitOptions.xli = satrec.xli;
        dsinitOptions.xni = satrec.xni;

        Dsinit dsinit =  new Dsinit();
        DsinitResult dsinitResult = new DsinitResult();
        dsinitResult = dsinit.dsinit(dsinitOptions); 

        satrec.irez = dsinitResult.irez;
        satrec.atime = dsinitResult.atime;
        satrec.d2201 = dsinitResult.d2201;
        satrec.d2211 = dsinitResult.d2211;

        satrec.d3210 = dsinitResult.d3210;
        satrec.d3222 = dsinitResult.d3222;
        satrec.d4410 = dsinitResult.d4410;
        satrec.d4422 = dsinitResult.d4422;
        satrec.d5220 = dsinitResult.d5220;

        satrec.d5232 = dsinitResult.d5232;
        satrec.d5421 = dsinitResult.d5421;
        satrec.d5433 = dsinitResult.d5433;
        satrec.dedt = dsinitResult.dedt;
        satrec.didt = dsinitResult.didt;

        satrec.dmdt = dsinitResult.dmdt;
        satrec.dnodt = dsinitResult.dnodt;
        satrec.domdt = dsinitResult.domdt;
        satrec.del1 = dsinitResult.del1;

        satrec.del2 = dsinitResult.del2;
        satrec.del3 = dsinitResult.del3;
        satrec.xfact = dsinitResult.xfact;
        satrec.xlamo = dsinitResult.xlamo;
        satrec.xli = dsinitResult.xli;

        satrec.xni = dsinitResult.xni;

      }

      // ----------- set variables if not deep space -----------
      if (satrec.isimp != 1) {
        cc1sq = satrec.cc1 * satrec.cc1;
        satrec.d2 = 4.0 * ao * tsi * cc1sq;
        temp = (satrec.d2 * tsi * satrec.cc1) / 3.0;
        satrec.d3 = ((17.0 * ao) + sfour) * temp;
        satrec.d4 = 0.5 * temp * ao * tsi * ((221.0 * ao) + (31.0 * sfour)) * satrec.cc1;
        satrec.t3cof = satrec.d2 + (2.0 * cc1sq);
        satrec.t4cof = 0.25 * ((3.0 * satrec.d3)
          + (satrec.cc1 * ((12.0 * satrec.d2) + (10.0 * cc1sq))));
        satrec.t5cof = 0.2 * (
          (3.0 * satrec.d4)
          + (12.0 * satrec.cc1 * satrec.d3)
          + (6.0 * satrec.d2 * satrec.d2)
          + (15.0 * cc1sq * ((2.0 * satrec.d2) + cc1sq))
        );
      }

      /* finally propogate to zero epoch to initialize all others. */
      // sgp4fix take out check to let satellites process until they are actually below earth surface
      // if(satrec.error == 0)

    }

    Sgp4 sgp4 = new Sgp4();

    sgp4.sgp4(satrec, 0);

    satrec.init = 'n';

    /* eslint-enable no-param-reassign */









    }



  }

}