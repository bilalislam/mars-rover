using System.Collections.Generic;

namespace LessonProgramSheet
{
    public class Point
    {
        //aynı saatte farklı hocalar aynı veya farklı dersleri verebilirler
        public List<Lesson> Lessons { get; set; }
        public Point Upper;
        public Point Bottom;
        public Point Left;
        public Point Right;
    }
}