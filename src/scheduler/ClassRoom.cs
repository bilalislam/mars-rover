using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class ClassRoom
    {
        public string Name { get; private set; }
        public Queue<Lesson> Lessons { get; private set; }
        public Sheet Sheet { get; private set; }
        public int TotalDailyLessonHour { get; private set; }


        private ClassRoom(string name, Queue<Lesson> lessons)
        {
            this.Name = name;
            this.Lessons = lessons;
        }

        /// <summary>
        /// Business invariants
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lessons"></param>
        /// <returns></returns>
        public static ClassRoom Load(string name, List<Lesson> lessons)
        {
            return new ClassRoom(name, SetLessonList(lessons));
        }

        private static Queue<Lesson> SetLessonList(List<Lesson> lessons)
        {
            var queueLessons = new Queue<Lesson>();
            foreach (var lesson in lessons)
            {
                queueLessons.Enqueue(lesson);
            }

            return queueLessons;
        }

        public ClassRoom SetSheet(Sheet sht)
        {
            Sheet = sht;
            return this;
        }

        public void Draw()
        {
            for (int i = 0; i < Sheet.Points.Length; i++)
            {
                for (int j = 0; j < Sheet.Points.Length; j++)
                {
                    var lesson = DequeueWithCalculatedHour();
                    var point = Sheet.Points[i, j];
                    SetLesson(point, lesson);
                }
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
        ///  5. Günlük ders saatleri 5 - nok
        ///  6. Son kalan dersin saatinin de günlük ders saatine ayarlanması lazım bu da tekrar hesap gerektirir - nok
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lesson"></param>
        private void SetLesson(Point point, Lesson lesson)
        {
            if (!point.Lessons.Any())
            {
                point.AddLesson(lesson
                    .SetTeacherName()
                    .SetClassName(this.Name));

                this.EnqueueIfRemained(lesson);
            }
            else
            {
                var currentLessons = new List<Lesson>();
                currentLessons.AddRange(point.Lessons);

                foreach (var lsn in currentLessons)
                {
                    if (lsn.ClassName == this.Name)
                    {
                        Lessons.Enqueue(lesson);
                        continue;
                        ;
                    }

                    if (lsn.Name == lesson.Name && !(lesson.Teachers.Count > 1))
                    {
                        Lessons.Enqueue(lesson);
                        SetLesson(point, DequeueWithCalculatedHour());
                    }
                    else if (lsn.Name == lesson.Name && lesson.Teachers.Count > 1)
                    {
                        point.AddLesson(lesson
                            .ChangeTeacherExtractWith(lsn.TeacherName)
                            .SetClassName(this.Name));

                        this.EnqueueIfRemained(lesson);
                    }
                    else
                    {
                        point.AddLesson(lesson
                            .SetTeacherName()
                            .SetClassName(this.Name));

                        this.EnqueueIfRemained(lesson);
                    }
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