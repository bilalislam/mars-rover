namespace rovers
{
    public class MoveEast : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Right == null){
                rover.CurrentPoint.IsRip = true;
                return rover;
            }

            if (rover.CurrentPoint.Right.IsRip && rover.Direction == Direction.E)
                return rover;

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Right.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Right;
            return rover;
        }
    }
}