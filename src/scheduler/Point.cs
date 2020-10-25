using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class Point
    {
        //aynı saatte farklı hocalar aynı veya farklı dersleri verebilirler
        public List<Lesson> Lessons { get; private set; }
        public Point Upper;
        public Point Bottom;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Lessons = new List<Lesson>();
        }

        public int X { get; private set; }
        public int Y { get; private set; }


        /// <summary>
        /// Suan 1 ya da 2 saatlik gelir
        /// max ise 2 dir bu business kararı
        /// eger 3 olursa 3 saate yayılabilir.
        /// TODO:Validasyonu unuttuk sanırım kontrolsüz de diger kolona eklemek düzeni bozdu galiba
        /// TODO: Bütün business burda olması lazım  ama queue'yu yonetemem onu düşün
        /// ve tüm kolonun derslerini de bilmem gerekir ki daha önce verilen dersleri kontrol edebileyim çakışma için
        /// peki çakışma varsa dequeue & enqueue için false doner o zaman class bunu dequeue eder true ise kalanı enqueue eder
        /// </summary>
        /// <param name="lesson"></param>
        public void AddLesson(Lesson lesson)
        {
            this.Lessons.Add(lesson);
            if (lesson.Hour > 1)
                this.Upper?.Lessons.Add(lesson);
        }

        public List<string> GetTeachers()
        {
            return Lessons.Select(lesson => lesson.TeacherName).ToList();
        }
    }
}