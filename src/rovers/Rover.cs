using System;
using System.Linq;

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
            if (cmd.Any(item => _letters.Contains(item))){
                this.Command = cmd;
            }
            else{
                throw new Exception("Invalid input");
            }
        }

        /// <summary>
        ///  run commands concurrently
        /// </summary>
        /// <param name="index"></param>
        public Rover RunCommand(int index){
            if (this.Command.Length > index){
                switch (Command[index]){
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

        /// <summary>
        /// out of plateau exception
        /// crush rover on same point 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Move(){
            switch (this.Direction){
                case Direction.E:
                    if (this.CurrentPoint.Right == null)
                        throw new Exception("Out of plateau !");

                    this.CurrentPoint.RoverOn = null;
                    this.CurrentPoint.Right.RoverOn = this;
                    this.CurrentPoint = this.CurrentPoint.Right;
                    break;
                case Direction.W:
                    if (this.CurrentPoint.Left == null)
                        throw new Exception("Out of plateau !");

                    this.CurrentPoint.RoverOn = null;
                    this.CurrentPoint.Left.RoverOn = this;
                    this.CurrentPoint = this.CurrentPoint.Left;
                    break;
                case Direction.N:
                    if (this.CurrentPoint.Upper == null)
                        throw new Exception("Out of plateau !");

                    this.CurrentPoint.RoverOn = null;
                    this.CurrentPoint.Upper.RoverOn = this;
                    this.CurrentPoint = this.CurrentPoint.Upper;
                    break;
                case Direction.S:
                    if (this.CurrentPoint.Bottom == null)
                        throw new Exception("Out of plateau !");

                    this.CurrentPoint.RoverOn = null;
                    this.CurrentPoint.Bottom.RoverOn = this;
                    this.CurrentPoint = this.CurrentPoint.Bottom;
                    break;
            }
        }

        public override string ToString(){
            return $"{this.CurrentPoint.X} {this.CurrentPoint.Y} {this.Direction}";
        }
    }
}