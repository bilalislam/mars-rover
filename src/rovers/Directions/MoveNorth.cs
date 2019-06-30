namespace rovers
{
    public class MoveNorth : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Upper != null){
                rover.CurrentPoint.RoverOn = null;
                rover.CurrentPoint.Upper.RoverOn = rover;
                rover.CurrentPoint = rover.CurrentPoint.Upper;
            }

            return rover;
        }
    }
}