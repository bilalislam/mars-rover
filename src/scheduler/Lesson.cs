using System.Collections.Generic;

namespace LessonProgramSheet
{
    public class Lesson
    {
        public string Name { get; set; }
        public int TotalHour { get; set; }
        public int Hour { get; set; }
        public int Used { get; set; }

        public List<string> Teacher { get; set; }
    }
}