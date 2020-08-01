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

      initlResult = initl.initl(initlOptions);
      
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











    }



  }

}