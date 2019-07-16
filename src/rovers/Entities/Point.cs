namespace rovers
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }
        public Direction Direction { get; set; }

        public Point Upper;
        public Point Bottom;
        public Point Left;
        public Point Right;
        public Rover RoverOn { get; set; }
        public bool IsRip { get; set; }

        /// <summary>
        /// x and y are representing to the position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y){
            X = x;
            Y = y;
        }
    }
}