namespace rovers
{
    public class MoveWest : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.IsRip && rover.CurrentPoint.Direction == Direction.W)
                return rover;

            if (rover.CurrentPoint.Left == null){
                rover.CurrentPoint.IsRip = true;
                return rover;
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Left.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Left;
            return rover;
        }
    }
}