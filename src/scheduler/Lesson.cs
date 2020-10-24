using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class Lesson
    {
        public string Name { get; set; }
        public int TotalHour { get; set; }
        public int Hour { get; set; }
        public int Used { get; set; }
        public List<string> Teachers { get; set; }
        public string TeacherName { get; private set; }
        public string ClassName { get; private set; }

        public Lesson ChangeTeacherExtractWith(string name)
        {
            var rnd = new Random();
            this.Teachers = this.Teachers.Where(x => x != name).ToList();
            var index = rnd.Next(0, this.Teachers.Count());
            this.TeacherName = this.Teachers[index - 1];
            return this;
        }

        public Lesson SetTeacherName()
        {
            this.TeacherName = this.Teachers.First();
            return this;
        }

        public Lesson SetClassName(string name)
        {
            ClassName = name;
            return this;
        }
    }
}