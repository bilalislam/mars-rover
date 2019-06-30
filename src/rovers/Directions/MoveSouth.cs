namespace rovers
{
    public class MoveSouth : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Bottom != null){
                rover.CurrentPoint.RoverOn = null;
                rover.CurrentPoint.Bottom.RoverOn = rover;
                rover.CurrentPoint = rover.CurrentPoint.Bottom;
            }

            return rover;
        }
    }
}