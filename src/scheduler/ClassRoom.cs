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
                        if (l.ClassName == this.Name)
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
        ///  4. Farklı dersleri veren aynı hocaların kendi diger dersleri ile çakışmaması gerekir. - ok
        ///  5. Günlük ders saatleri 5 - ok
        ///  6. Son kalan dersin saatinin de günlük ders saatine ayarlanması lazım bu da tekrar hesap gerektirir - ok
        ///  7. Sorun dersler parçalanıyor. - ignored -tekli derslerden seçilebilir.
        ///  8. Hocaların verdiği toplam ders saatleri - nok
        ///  9. ders saatleri  farklı ama sınıf isimleri aynı olmaması lazım o da aynı gelmiş 12. madde ? - ok 
        ///  10. eksik dersler gördüm - ok
        ///  11. 2b sınıfının neden 5 saatten fazla dersi var ? - 12. madde ? - ok
        ///  12. birden fazla hoca da aynı anda aynı dersi verdikleri zaman gelen aynı ders ignore edilmesi lazım çünkü hocaların hiçbiri müsait değil
        /// veya hocaların farklı dersleri var ama dersin başka hocası yokda requeue etmeli varsa diger hocadan devam etmeli - ok
        ///  13. hoca 2 ayrı dersleri ortak verirler dead lock olabilir ama suan datalarda bu durum yok.
        ///  14. beden dersinin hocası aynı kendi saatleri ile çakıştı ona bak , birden fazla hoca koysam ya da yeni
        /// hoca versem ya overflow veriyor kendisi kaldıgı için sadece q'da ya da q empty ??
        /// q'ya fazladan ders sokmamak lazım ya da fazla'dan kullanıyormus gibi olursa totalhour'u geçer
        /// lan bir logic kuramadık be
        /// ana sorun 2. index'i tekrar validate ederek set etmek ve bunu yaparken q'da az veya cok eklememek ve birden fazla
        /// loop'a sokmamak !
        /// her attıgım adımın bir sonra kini düşünmem lazım
        /// retry ederken return olması lazım çünkü queue'ya bitirir
        /// setlesson'ı tekrar yazmak lazım sanki
        /// dynamic programming eksikliklerim var buna calıs !!!
        /// 2 kere enqueue yapıyorum recursive yaparken bunu çöz !??
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

                if (currentLessons.Any(x => x.ClassName == lesson.ClassName))
                {
                    this.EnqueueIfRemained(lesson, false);
                    return;
                }

                if (currentLessons.Any(x => x.TeacherName == lesson.TeacherName))
                {
                    this.Requeue(point, lesson);
                    return;
                }


                point.AddLesson(lesson
                    .SetClassName(this.Name));

                this.EnqueueIfRemained(lesson);
            }
        }

        private void Requeue(Point point, Lesson lesson)
        {
            if (lesson.Teachers.Count > 1)
            {
                var currentLesson = point.Lessons.FirstOrDefault(x => x.TeacherName == lesson.TeacherName);

                lesson.ChangeTeacherExtractWith(currentLesson.TeacherName);
                SetLesson(point, lesson);
            }
            else
            {
                this.EnqueueIfRemained(lesson, false);
                SetLesson(point, DequeueWithCalculatedHour());
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