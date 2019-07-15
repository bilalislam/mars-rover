using rovers.Exceptions;

namespace rovers
{
    public class MoveSouth : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Bottom == null){
                throw new OutOfPlateauException("Out of Plateau");
            }

            if (rover.CurrentPoint.Bottom.RoverOn != null){
                throw new CrushDetectedException("Crush detected");
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Bottom.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Bottom;

            return rover;
        }
    }
}