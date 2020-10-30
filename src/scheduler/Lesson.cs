using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class Lesson
    {
        public string Name { get; private set; }
        public int TotalHour { get; private set; }
        public int Hour { get; private set; }
        public int Used { get; private set; }
        public List<string> Teachers { get; private set; }
        public string TeacherName { get; private set; }
        public string ClassName { get; private set; }
        public int Rate { get; private set; }

        private Lesson(string name, int totalHour, List<string> teachers)
        {
            this.Name = name;
            this.Teachers = teachers;
            this.TotalHour = totalHour;
        }

        public static Lesson Load(string name, int totalHour, List<string> teachers)
        {
            return new Lesson(name, totalHour, teachers)
                .SetTeacherName();
        }

        public Lesson ChangeTeacherExtractWith(string name)
        {
            var rnd = new Random();
            var teachers = this.Teachers.Where(x => x != name).ToList();
            var index = rnd.Next(1, teachers.Count);
            this.TeacherName = teachers[index - 1];
            return this;
        }

        private Lesson SetTeacherName()
        {
            this.TeacherName = this.Teachers.First();
            return this;
        }

        public Lesson SetClassName(string name)
        {
            ClassName = name;
            return this;
        }

        public Lesson SetHour(int hour)
        {
            this.Hour = hour;
            return this;
        }

        public Lesson SumUsed(int hour)
        {
            this.Used += hour;
            return this;
        }

        public Lesson IncreaseRate()
        {
            ++Rate;
            return this;
        }
    }
}