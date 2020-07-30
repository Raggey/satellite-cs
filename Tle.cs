    
  
  using System.Text.RegularExpressions;
    
  public class Tle {

    public string line0;
    public string[] line1;
    public string[] line2;
    public string name;
    public string catalogNumber;
    public string classification;
    public string designator;
    public string epoch; 
    public string meanMotion_fd; //First derivative, aka // Ballastic Coefficient
    public string meanMotion_sd; //second derivative
    public string dragTerm; // Drag Term, radiation Pressure Co or BSTAR 
    public string ephemerisType;
    public string elementSetNumber;
    public string checksum1;
    public string catalogNumber2;

    public string inclination;
    public string rightAscension;
    public string eccentricity;
    public string argumentPerigee;
    public string meanAnomaly;
    public string meanMotion;
    public string revNumAtEpoch;
    public string checksum2;

    public Tle(string line0, string line1, string line2){

      this.line0 = line0;
      this.line1 = CleanWhitespace(line1).Split(' ');
      this.line2 = CleanWhitespace(line2).Split(' ');

      if(!ParseTle()) {
        throw new System.ArgumentException("Parameter error with TLE data", "line 2"); 
      }

    }

   
    public bool ParseTle(){

      name = line0.Trim();
      catalogNumber = line2[1]; 
      classification = line1[1].Substring( line1[1].Length -1  ); // U in 25544U
      designator = line1[2];
      epoch = line1[3]; //TODO: Implement some logic for Epoch year generation.. 57-99 correspond to 1957-1999 and those from 00-56 correspond to 2000-2056. 
      meanMotion_fd = line1[4]; 
      meanMotion_sd = line1[5];
      dragTerm = line1[6];
      ephemerisType = line1[7];
      elementSetNumber = line1[8].Substring(0, line1[8].Length -1);
      checksum1 = line1[8].Substring(line1[8].Length); // Final digit

      catalogNumber2 = line2[1];
      inclination = line2[2];
      rightAscension = line2[3];
      eccentricity = line2[4];
      argumentPerigee = line2[5];
      meanAnomaly = line2[6];

      // Check format of Line2 of TLE and parse 
      if(line2.Length == 8) {
        // Last digits together 
        meanMotion = line2[7].Substring(0,11); // First 10
        revNumAtEpoch = line2[7].Substring(11,5); // Next 5
        checksum2 = line2[7].Substring(16); // Final 1
      }
      else if(line2.Length == 9) {
        
        meanMotion = line2[7];
        revNumAtEpoch = line2[8].Substring(0, line2[8].Length - 1);
        checksum2 = line2[8].Substring(line2[8].Length - 1); // Final index
      }
      else {
        return false;
      }

      return true;

    }
     

    string CleanWhitespace(string str) {
      // replace multiple whitespace
      return Regex.Replace(str, @"\s+", " ");
    }






    














  }




  // https://spaceflight.nasa.gov/realdata/sightings/SSapplications/Post/JavaSSOP/SSOP_Help/tle_def.html
 // https://en.wikipedia.org/wiki/Two-line_element_set
