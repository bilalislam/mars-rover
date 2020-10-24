using System.Collections.Generic;

namespace Scheduler
{
    public class Point
    {
        //aynı saatte farklı hocalar aynı veya farklı dersleri verebilirler
        public List<Lesson> Lessons { get; private set; } = new List<Lesson>();
        public Point Upper;
        public Point Bottom;

        public void AddLesson(Lesson lesson)
        {
            int hour = lesson.Hour - 1;
            this.Lessons.Add(lesson);
            Upper?.Lessons.Add(lesson);
        }
    }
}