namespace rovers
{
    public class MoveWest : IMove
    {
        public Rover Execute(Rover rover){
            if (rover.CurrentPoint.Left == null){
                rover.CurrentPoint.IsRip = true;
                return rover;
            }

            if (rover.CurrentPoint.Left.IsRip && rover.Direction == Direction.W){
                return rover;
            }

            rover.CurrentPoint.RoverOn = null;
            rover.CurrentPoint.Left.RoverOn = rover;
            rover.CurrentPoint = rover.CurrentPoint.Left;
            return rover;
        }
    }
}