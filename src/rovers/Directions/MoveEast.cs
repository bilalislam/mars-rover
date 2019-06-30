namespace rovers
{
    public class MoveEast : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Right != null){
                rover.CurrentPoint.RoverOn = null;
                rover.CurrentPoint.Right.RoverOn = rover;
                rover.CurrentPoint = rover.CurrentPoint.Right;
            }

            return rover;
        }
    }
}