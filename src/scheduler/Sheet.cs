using System.Collections.Generic;

namespace LessonProgramSheet
{
    public class Sheet
    {
        private Point[,] Points { get; set; }
        private List<ClassRoom> ClassRooms { get; set; }

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

                    if (i > 0)
                    {
                        point.Left = this.Points[i - 1, j];
                        point.Left.Right = point;
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