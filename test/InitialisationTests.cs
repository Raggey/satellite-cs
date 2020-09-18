// using Xunit;
// using Sat = Satellite_cs;

// namespace UnitTests
// {
//     public class InitialisationTests
//     {



//         [Fact]
//         public void CheckInitialisation(){
//           string line1 = "1 25544U 98067A   20206.38292522 -.00000985  00000-0 -95291-5 0  9998";
//           string line2 = "2 25544  51.6430 164.3636 0001088 140.8410 323.1994 15.49511774237787";
        
//           Sat.Sat_Io io = new Sat.Sat_Io();
//           Sat.Satrec satrec = io.twoline2satrec(line1,line2);

//           Sat.Sgp4 sgp4 = new Sat.Sgp4();
//           Sat.PositionAndVelocity positionAndVelocity = sgp4.sgp4(satrec, 0);

//           Assert.Equal( 485.6716711104389, positionAndVelocity.position_ECI.x ); // Value from Sat.js
//           Assert.Equal(-4381.651985814846, positionAndVelocity.position_ECI.y ); // Value from Sat.js
//           Assert.Equal(5162.784591395867, positionAndVelocity.position_ECI.z  ); // Value from Sat.js
//         }
//     }
// }
