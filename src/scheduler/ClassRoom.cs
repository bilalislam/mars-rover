using System.Collections.Generic;

namespace Scheduler
{
    public class ClassRoom
    {
        public string Name { get; private set; }
        public Queue<Lesson> Lessons { get; private set; }
        public Sheet Sheet { get; private set; }


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

        public ClassRoom SetSheet(Sheet sht)
        {
            Sheet = sht;
            return this;
        }

        public void Draw()
        {
            while (this.Lessons.Count > 0)
            {
                var lesson = DequeueWithCalculatedHour();
                var point = Sheet.Points[0, 0];

                SetLesson(point, lesson);
            }
        }

        /// <summary>
        ///  1. çakışma varsa
        ///  1.1 farklı hoca varsa onu kullan ( group and min)' e gore dagılım yap (şimdilik random !)
        ///  1.2 yoksa sadece enqueue et o zaman tekrar başka bir uygun ders bulana kadar Dequeue & Enqueue etmem lazım yani recursive
        ///  2. çakışma yoksa
        ///  2.1 ders ve hoca farklı demektir set et yukarda ki gibi ve  bu işlem ise recursive olabilir.
        ///  3. Birden fazla hocası olan derslerde sınıf bazında hangi hoca ile başlandıysa onun ile bitirilmesi gerekir. - nok
        ///  4. Farklı dersleri veren aynı hocaların kendi diger dersleri ile çakışmaması gerekir. - ignored 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lesson"></param>
        public void SetLesson(Point point, Lesson lesson)
        {
            foreach (var lsn in point.Lessons)
            {
                if (lsn.Name == lesson.Name && !(lesson.Teachers.Count > 1))
                {
                    Lessons.Enqueue(lesson);
                    SetLesson(point, DequeueWithCalculatedHour());
                }
                else if (lsn.Name == lesson.Name && lesson.Teachers.Count > 1)
                {
                    point.AddLesson(lesson
                        .ChangeTeacherExtractWith(lsn.TeacherName));
                    this.EnqueueIfRemained(lesson);
                }
                else
                {
                    point.AddLesson(lesson
                        .SetTeacherName());
                    this.EnqueueIfRemained(lesson);
                }
            }
        }

        private Lesson DequeueWithCalculatedHour()
        {
            var lesson = Lessons.Dequeue();
            lesson.Hour = lesson.Hour switch
            {
                0 => 2,
                1 => 1,
                _ => 2
            };

            return lesson;
        }

        private void EnqueueIfRemained(Lesson lesson)
        {
            lesson.Used += lesson.Hour;
            if (lesson.Used == lesson.TotalHour) return;
            lesson.Hour = lesson.TotalHour - lesson.Used;
            Lessons.Enqueue(lesson);
        }
    }
}