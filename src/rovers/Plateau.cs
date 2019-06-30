using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rovers {
    public class Plateau : IPlateu {
        private readonly int _x;
        private readonly int _y;
        public Point[, ] _points;
        private readonly List<Rover> _rovers;

        /// <summary>
        /// x and y are representing to the dimensions
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Plateau (int x, int y) {
            this._x = x;
            this._y = y;
            this._points = new Point[x, y];
            this._rovers = new List<Rover> ();

            Init ();
        }

        /// <summary>
        /// Draw all space of platau
        /// all point knows all neighbours of itself
        /// </summary>
        private void Init () {
            for (int i = 0; i < this._x; i++) {
                for (int j = 0; j < this._y; j++) {
                    var point = new Point (i, j);
                    this._points[i, j] = point;
                    if (j > 0) {
                        point.Left = this._points[i, j - 1];
                        point.Left.Right = point;
                    }

                    if (i > 0) {
                        point.Upper = this._points[i - 1, j];
                        point.Upper.Bottom = point;
                    }
                }
            }
        }

        public void AddRover (Rover rover) {
            if (!(rover.X >= this._x || rover.Y >= this._y)) {
                this._rovers.Add (rover);
                SetRoverPoint (rover, rover.X, rover.Y);
            } else {
                throw new Exception ("Invalid rover co-ordinates !");
            }
        }

        public void Run () {
            if (!this._rovers.Any ()) {
                throw new Exception ("Any rovers can not found !");
            }

            var maxTaskCount = this._rovers.Max (x => x.Command).Length;
            for (int i = 0; i < maxTaskCount; i++) {
                foreach (var rover in this._rovers) {
                    rover.RunCommand (i);
                }
            }

            RoverConsoleOutput ();
        }

        private void RoverConsoleOutput () {
            foreach (var rover in this._rovers) {
                Console.WriteLine (rover.ToString ());
            }
        }

        /// <summary>
        /// rovers knows which point
        /// point knows who rovers on itself
        /// </summary>
        /// <param name="rover"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetRoverPoint (Rover rover, int x, int y) {
            rover.CurrentPoint = this._points[x, y];
            this._points[x, y].RoverOn = rover;
        }

        public override string ToString () {
            var sb = new StringBuilder ();
            for (int i = 0; i < this._x; i++) {
                for (int j = 0; j < this._y; j++) {
                    sb.AppendLine ($"x : {this._points[i, j].X} " +
                        $"y : {this._points[i, j].Y} " +
                        $"rover: {this._points[i, j].RoverOn?.CurrentPoint.X} <-> {this._points[i, j].RoverOn?.CurrentPoint.Y} " +
                        $"=> " +
                        $" bottom : {this._points[i, j].Bottom?.X} <-> {this._points[i, j].Bottom?.Y} " +
                        $" upper  : {this._points[i, j].Upper?.X}  <-> {this._points[i, j].Upper?.Y} " +
                        $" left   : {this._points[i, j].Left?.X}   <-> {this._points[i, j].Left?.Y}" +
                        $" right  : {this._points[i, j].Right?.X}  <-> {this._points[i, j].Right?.Y}");
                }
            }

            return sb.ToString ();
        }
    }
}