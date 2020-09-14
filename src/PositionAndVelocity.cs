using System;


namespace Satellite_cs{


  public class PositionAndVelocity {

    public Coordinates position_ECI;
    public Coordinates velocity_ECI;


    public PositionAndVelocity(){

      position_ECI = new Coordinates();
      velocity_ECI = new Coordinates();

    }

    

  }


}
