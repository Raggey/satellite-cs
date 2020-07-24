    
  
  using System.Text.RegularExpressions;
    
    
  public class Tle {


    public string line0;
    public string[] line1;
    public string[] line2;

    public string name;
    public string catalogNumber;
    public string classification;
    public string designator;
    public string elementNoCheckSum;

    public string epoch; 


    public string meanMotion_fd; //First derivative, aka // Ballastic Coefficient
    public string meanMotion_sd; //second derivative
    public string dragTerm; // Drag Term, radiation Pressure Co, BSTAR 
    public string ephemerisType;

    public float inclination;


    public Tle(string line0, string line1, string line2){

      this.line0 = line0;
      this.line1 = CleanWhitespace(line1).Split(' ');
      this.line2 = CleanWhitespace(line2).Split(' ');

      ParseTle();

    }

   


    public void ParseTle(){

      name = line0.Trim();
      catalogNumber = line2[1]; 
      classification = line1[1].Substring( line1[1].Length -1  ); // U in 25544U
      designator = line1[2];
      epoch = line1[3]; //TODO: Implement some logic for Epoch year generation
      meanMotion_fd = line1[4]; 
      meanMotion_sd = line1[5];
      dragTerm = line1[6];
      ephemerisType = line1[7];
      elementNoCheckSum = line1[8];

      


    }




      

    string CleanWhitespace(string str) {
      // replace multiple whitespace
      return Regex.Replace(str, @"\s+", " ");
    }
  }




  // https://spaceflight.nasa.gov/realdata/sightings/SSapplications/Post/JavaSSOP/SSOP_Help/tle_def.html
 // https://en.wikipedia.org/wiki/Two-line_element_set
