using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    /// <summary>
    /// Building blocks of model driven design !!!
    /// </summary>
    public class ClassRoom
    {
        public string Name { get; private set; }
        public Queue<Lesson> Lessons { get; private set; }
        public Sheet Sheet { get; private set; }
        public int CurrentDailyHours { get; private set; }
        public int TotalDailyHours { get; private set; }

        private ClassRoom(string name, int totalDailyHours)
        {
            this.Name = name;
            this.TotalDailyHours = totalDailyHours;
        }

        public static ClassRoom Load(string name, int totalDailyHours)
        {
            return new ClassRoom(name, totalDailyHours);
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
                lesson.SetClassName(this.Name);
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

                this.CurrentDailyHours = 0;
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
        ///  9. ders saatleri  farklı ama sınıf isimleri aynı olmaması lazım o da aynı gelmiş 12. madde ?
        ///  10. eksik dersler gördüm - ok
        ///  11. 2b sınıfının neden 5 saatten fazla dersi var ? - 12. madde ? 
        ///  12. birden fazla hoca da aynı anda aynı dersi verdikleri zaman gelen aynı ders ignore edilmesi lazım çünkü hocaların hiçbiri müsait değil
        ///  13. rule engine must !!
        ///
        /// 1. 1A sınıfı dersi yayarken kendisi ile çakışabilir
        /// 2. 1a ve 2b nin dersleri çakışabilir.
        /// 3. birden fazla dersi veren hocalar kendileri ile çakışabilir
        /// fazla hocası olan aynı dersleri dagıt
        /// hocaları aynı olan farklı dersleri dagıt
        /// hocaların birbirine yakın ders saat vermelerini sagla
        /// 4. hangi sınıf hangi hoca ile başladıysa onunda tamamla
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lesson"></param>
        private void SetLesson(Point point, Lesson lesson)
        {
            if (!point.Lessons.Any())
            {
                point.AddLesson(lesson
                    .SetClassName(this.Name));

                this.EnqueueIfRemained(lesson);
            }
            else
            {
                var currentLessons = new List<Lesson>();
                currentLessons.AddRange(point.Lessons);

                var valid = true;
                if (currentLessons.Any(x => x.ClassName == lesson.ClassName))
                {
                    this.EnqueueIfRemained(lesson, false);
                    return;
                }

                if (currentLessons.Any(x => x.Name == lesson.Name))
                {
                    valid = false;
                    this.EnqueueIfRemained(lesson, false);
                    SetLesson(point, DequeueWithCalculatedHour());
                }


                if (currentLessons.Any(x => x.TeacherName == lesson.TeacherName))
                {
                    valid = false;
                    this.EnqueueIfRemained(lesson, false);
                    SetLesson(point, DequeueWithCalculatedHour());
                }

                if (valid)
                {
                    point.AddLesson(lesson
                        .SetClassName(this.Name));

                    this.EnqueueIfRemained(lesson);
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
            var hour = lesson.Hour switch
            {
                0 => 2,
                1 => 1,
                _ => 2
            };

            lesson.SetHour(hour);
            var remainedHours = Math.Abs(this.CurrentDailyHours - this.TotalDailyHours);
            if (remainedHours < lesson.Hour && remainedHours != 0)
                lesson.SetHour(remainedHours);

            this.CurrentDailyHours += lesson.Hour;
            return lesson;
        }

        private void EnqueueIfRemained(Lesson lesson, bool isUsed = true)
        {
            if (!isUsed)
            {
                this.CurrentDailyHours -= lesson.Hour;
                lesson.SetHour(0);
            }

            lesson.SumUsed(lesson.Hour);
            if (lesson.Used == lesson.TotalHour) return;
            lesson.SetHour(lesson.TotalHour - lesson.Used);
            Lessons.Enqueue(lesson);
        }
    }
}