using System;
using System.Collections.Generic;

namespace LessonProgramSheet
{
    public class ClassRoom
    {
        public string Name { get; private set; }
        public Queue<Lesson> Lessons { get; private set; }

        private ClassRoom(string name)
        {
            this.Name = name;
        }

        public static ClassRoom Load(string name)
        {
            return new ClassRoom(name);
        }

        public ClassRoom SetLessonList(List<Lesson> lessons)
        {
            Lessons = new Queue<Lesson>();
            foreach (var lesson in lessons)
            {
                Lessons.Enqueue(lesson);
            }

            return this;
        }

        public void Draw()
        {
            while (this.Lessons.Count > 0)
            {
                var lesson = Lessons.Dequeue();
                lesson.Hour = lesson.Hour switch
                {
                    0 => 2,
                    1 => 1,
                    _ => 2
                };

                //pointlere yerleştir
                // Eger çakışma varsa dersin başka hocası var ise onu kullan
                // Çakışma var ama hoca yok ise enqueue yap
                // Dersin birden fazla hocası var o sınıf için daha önce alınan ders ise diger yarısıdır aynı hocadan devam et.
                // Dersin hocalarının oranları - Group & Min
                for (int j = 0; j < lesson.Hour; j++)
                {
                    Console.WriteLine(lesson.Name);
                }

                //engueue
                lesson.Used += lesson.Hour;
                if (lesson.Used != lesson.TotalHour)
                {
                    lesson.Hour = lesson.TotalHour - lesson.Used;
                    Lessons.Enqueue(lesson);
                }
            }
        }
    }
}