using System.Collections.Generic;

namespace Scheduler
{
    public class Sheet
    {
        public Point[,] Points { get; private set; }
        public List<ClassRoom> ClassRooms { get; private set; }

        /// <summary>
        /// Weekly lessons program
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Sheet(int x, int y)
        {
            Points = new Point[x + 1, y + 1];
            ClassRooms = new List<ClassRoom>();

            Init(x, y);
        }

        private void Init(int x, int y)
        {
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    var point = new Point();
                    Points[i, j] = point;
                    if (j > 0)
                    {
                        point.Bottom = this.Points[i, j - 1];
                        point.Bottom.Upper = point;
                    }
                }
            }
        }

        public void AddClass(ClassRoom classRoom)
        {
            this.ClassRooms.Add(classRoom);
        }
    }
}