using System.Collections.Generic;

namespace rovers
{
    public class MoveAction
    {
        private readonly Dictionary<Direction, IMove> _moveActions = new Dictionary<Direction, IMove>();

        public MoveAction(){
            _moveActions.Add(Direction.E, new MoveEast());
            _moveActions.Add(Direction.W, new MoveWest());
            _moveActions.Add(Direction.N, new MoveNorth());
            _moveActions.Add(Direction.S, new MoveSouth());
        }

        public Rover Execute(Rover rover){
            return _moveActions[rover.Direction].Execute(rover);
        }
    }
}