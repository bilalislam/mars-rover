using rovers.Exceptions;

namespace rovers
{
    public class MoveNorth : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Upper == null){
                throw new OutOfPlateauException("Out of Plateau");
            }

            if (rover.CurrentPoint.Upper.RoverOn != null){
                throw new CrushDetectedException("Crush detected");
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Upper.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Upper;

            return rover;
        }
    }
}