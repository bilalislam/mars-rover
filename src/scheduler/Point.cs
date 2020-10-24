using System.Collections.Generic;

namespace Scheduler
{
    public class Point
    {
        //aynı saatte farklı hocalar aynı veya farklı dersleri verebilirler
        public List<Lesson> Lessons { get; private set; }
        public Point Upper;
        public Point Bottom;
        public Point Left;
        public Point Right;


        public void AddLesson(Lesson lesson)
        {
            this.Lessons.Add(lesson);
        }
    }
}