using System.Linq;
using rovers.Exceptions;

namespace rovers
{
    public class Rover : IRover
    {
        /* * L = Left  * R = Right * M = Move*/
        private readonly char[] _letters = {'L', 'R', 'M'};

        public int X;
        public int Y;
        public string Command { get; private set; }
        public Direction Direction { get; private set; }
        public Point CurrentPoint;

        public Rover(int x, int y, Direction direction, string cmd){
            this.X = x;
            this.Y = y;
            this.Direction = direction;
            this.SetCommand(cmd);
        }

        private void SetCommand(string cmd){
            if (cmd.Any(item => _letters.Contains(item)))
                this.Command = cmd;
            else
                throw new InvalidArgumentException("Invalid input");
        }

        public Rover RunCommand(){
            foreach (var t in Command){
                switch (t){
                    case 'L':
                        TurnLeft();
                        break;
                    case 'R':
                        TurnRight();
                        break;
                    case 'M':
                        Move();
                        break;
                }
            }

            return this;
        }

        private void TurnLeft(){
            if (this.Direction == Direction.E) this.Direction = Direction.N;
            else if (this.Direction == Direction.N) this.Direction = Direction.W;
            else if (this.Direction == Direction.W) this.Direction = Direction.S;
            else if (this.Direction == Direction.S) this.Direction = Direction.E;
        }

        private void TurnRight(){
            if (this.Direction == Direction.E) this.Direction = Direction.S;
            else if (this.Direction == Direction.S) this.Direction = Direction.W;
            else if (this.Direction == Direction.W) this.Direction = Direction.N;
            else if (this.Direction == Direction.N) this.Direction = Direction.E;
        }

        private void Move(){
            var action = new MoveAction();
            action.Execute(this);
        }

        public override string ToString(){
            return $"{this.CurrentPoint.X} {this.CurrentPoint.Y} {this.Direction}";
        }
    }
}