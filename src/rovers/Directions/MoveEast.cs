using rovers.Exceptions;

namespace rovers
{
    public class MoveEast : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Right == null){
                throw new OutOfPlateauException("Out of Plateau");
            }

            if (rover.CurrentPoint.Right.RoverOn != null){
                throw new CrushDetectedException("Crush detected");
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Right.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Right;

            return rover;
        }
    }
}