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
        public Direction Direction;
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

        public void RunCommand(int index){
            if (this.Command.Length > index){
                //do something
            }
        }

        private void TurnLeft(){
        }

        private void TurnRight(){
        }

        /// <summary>
        /// out of plateau exception
        /// crus rover on same point 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Move(){
        }
    }
}