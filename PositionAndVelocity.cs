using System;


namespace Satellite_cs{


  public class PositionAndVelocity {

    public Coordiantes position_ECI;
    public Coordiantes velocity_ECI;


    public PositionAndVelocity(){

      position_ECI = new Coordiantes();
      velocity_ECI = new Coordiantes();

    }

    

  }


}
