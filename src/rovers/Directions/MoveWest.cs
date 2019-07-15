using rovers.Exceptions;

namespace rovers
{
    public class MoveWest : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Left == null){
                throw new OutOfPlateauException("Out of Plateau");
            }

            if (rover.CurrentPoint.Left.RoverOn != null){
                throw new CrushDetectedException("Crush detected");
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Left.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Left;

            return rover;
        }
    }
}