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

        public void RunCommand(){
            throw new NotImplementedException();
        }


        private void TurnLeft(){
            throw new NotImplementedException();
        }

        private void TurnRight(){
            throw new NotImplementedException();
        }

        /// <summary>
        /// mevcut posizyon boyutu aşarsa R.I.P.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Move(){
            throw new NotImplementedException();
        }
    }
}