using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class ClassRoom
    {
        public string Name { get; private set; }
        public Queue<Lesson> Lessons { get; private set; }
        public Sheet Sheet { get; private set; }
        public int TotalDailyHours { get; private set; }


        private ClassRoom(string name)
        {
            this.Name = name;
        }


        public static ClassRoom Load(string name)
        {
            return new ClassRoom(name);
        }

        /// <summary>
        /// Tüm dersleri böl
        /// 4 ise 1-1-1-1
        /// 3 ise 1-1-1 
        /// 2 ise 1-1
        /// bölersen ve ardısık olursa bunlar sorun olmaz sanırım
        /// ama rassallıgı kaybederiz Block dersler gelir
        /// shuffle edersek çok kopuk olursa dersler mat 1 -tr1-cog-1-mat1 gibi 
        /// </summary>
        /// <param name="lessons"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Burada aslında business class'ın olmaması lazım yani
        /// class ders ekler bunun validasyonlarını sheet bilmesi lazım
        /// o zaman daha clean olur
        /// </summary>
        public void Draw()
        {
            var day = DayOfWeek.Monday;
            for (int i = 0; i < Sheet.Points.GetLength(0); i++)
            {
                Console.WriteLine("---------\n");
                Console.WriteLine($"{day.ToString()} ");
                Console.WriteLine("---------\n");
                ++day;

                for (int j = 0; j < Sheet.Points.GetLength(1); j++)
                {
                    var lesson = DequeueWithCalculatedHour();
                    var point = Sheet.Points[i, j];
                    SetLesson(point, lesson);
                    foreach (var l in point.Lessons)
                    {
                        Console.WriteLine($"{l.ClassName} - {l.Name} - {l.TeacherName} - {point.X} - {point.Y}");
                    }
                }

                this.TotalDailyHours = 0;
            }
        }


        /// <summary>
        ///  1. çakışma varsa
        ///  1.1 farklı hoca varsa onu kullan ( group and min)' e gore dagılım yap (şimdilik random !) - ok 
        ///  1.2 yoksa sadece enqueue et o zaman tekrar başka bir uygun ders bulana kadar Dequeue & Enqueue etmem lazım yani recursive  - ok
        ///  2. çakışma yoksa
        ///  2.1 ders ve hoca farklı demektir set et yukarda ki gibi ve  bu işlem ise recursive olabilir. - ok
        ///  3. Birden fazla hocası olan derslerde sınıf bazında hangi hoca ile başlandıysa onun ile bitirilmesi gerekir. - ignored
        ///  4. Farklı dersleri veren aynı hocaların kendi diger dersleri ile çakışmaması gerekir. - ignored
        ///  5. Günlük ders saatleri 5 - ok
        ///  6. Son kalan dersin saatinin de günlük ders saatine ayarlanması lazım bu da tekrar hesap gerektirir - ok
        ///  7. Sorun dersler parçalanıyor. - ignored
        ///  8. Hocaların verdiği toplam ders saatleri - nok
        ///  9. ders saatleri  farklı ama sınıf isimleri aynı olmaması lazım o da aynı gelmiş
        ///  10. eksik dersler gördüm
        ///  11. 2b sınıfının neden 5 saatten fazla dersi var ? 
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
                        this.TotalDailyHours -= lesson.Hour;
                        this.EnqueueIfRemained(lesson, false);
                        continue;
                    }

                    if (lsn.Name == lesson.Name && !(lesson.Teachers.Count > 1))
                    {
                        this.TotalDailyHours -= lesson.Hour;
                        this.EnqueueIfRemained(lesson, false);
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

        /// <summary>
        /// TODO: 2 saatlik dersler parçalandı :(
        /// Tekli sayılardan alırsam aslında 2-2 'lik dersleri bölmem ama suan gerek yok
        /// </summary>
        /// <returns></returns>
        private Lesson DequeueWithCalculatedHour()
        {
            var lesson = Lessons.Dequeue();
            lesson.Hour = lesson.Hour switch
            {
                0 => 2,
                1 => 1,
                _ => 2
            };

            var remainedHours = Math.Abs(this.TotalDailyHours - 5);
            if (remainedHours < lesson.Hour && remainedHours != 0)
                lesson.Hour = remainedHours;

            this.TotalDailyHours += lesson.Hour;
            return lesson;
        }

        private void EnqueueIfRemained(Lesson lesson, bool isUsed = true)
        {
            if (!isUsed)
                lesson.Hour = 0;

            lesson.Used += lesson.Hour;
            if (lesson.Used == lesson.TotalHour) return;
            lesson.Hour = lesson.TotalHour - lesson.Used;
            Lessons.Enqueue(lesson);
        }
    }
}