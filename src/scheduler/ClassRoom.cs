using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Building blocks of model driven design !!!
    /// </summary>
    public class ClassRoom
    {
        public string Name { get; private set; }
        public List<Lesson> Lessons { get; private set; }
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
            Lessons = new List<Lesson>();
            foreach (var lesson in lessons)
            {
                lesson.SetClassName(this.Name);
                Lessons.Add(lesson);
            }

            //Lessons = Shuffle(lessons);
            return this;
        }

        public List<T> Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list.ToList();
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
        ///
        /// 2 kere call edilirse false olan isSet true set edilir ve upper'a yazmaya calısır bu da karışmasına sebep olur
        /// eger içerde olsa bu sefer kullanılmayan dersi kullanıldı diye override ediyor recursive oldugu için 
        /// dequeu yaparken recursive olmaması lazım ama nasıl ??
        ///true'yu bulana kadar calıstıgı için sonucu false gelmez sonucuna gore logic kurmamak lazım.
        ///true geldiğinde false olması gereken ders için de çalışıyor !!
        ///mutlaka kullanılmıs olması şartı ile upper'a yazabilirsin
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
                    if (lesson != null)
                    {
                        var point = Sheet.Points[i, j];

                        var hour = lesson.Hour;
                        SetLesson(point, lesson);
                        var isUsed = this.EnqueueIfRemained(lesson);

                        if (isUsed && hour > 1 && point.Upper != null)
                        {
                            SetLesson(point.Upper, lesson);
                        }


                        foreach (var l in point.Lessons)
                        {
                            if (l.ClassName == this.Name)
                                Console.WriteLine(
                                    $"{l.ClassName} - {l.Name} - {l.TeacherName} - {point.X} - {point.Y}");
                        }
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
        /// dynamic programming eksikliklerim var buna calıs !
        /// 2 kere enqueue yapıyorum recursive yaparken bunu çöz ? 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lesson"></param>
        private bool SetLesson(Point point, Lesson lesson, bool flag = true)
        {
            if (!point.Lessons.Any())
            {
                point.AddLesson(lesson
                    .SetClassName(this.Name));

                return true;
            }

            var currentLessons = point.Lessons;
            if (currentLessons.Any(x => x.ClassName == lesson.ClassName))
            {
                this.MarkUnUsed(lesson)
                    .IncreaseRate();

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in currentLessons)
                {
                    Console.WriteLine(
                        $"Ç : {lesson.ClassName} - {lesson.Name} - {lesson.TeacherName} - {point.X} - {point.Y}  | {item.ClassName} - {item.Name} - {item.TeacherName}");
                }

                Console.ResetColor();

                return false;
            }

            if (currentLessons.Any(x => x.TeacherName == lesson.TeacherName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in currentLessons)
                {
                    Console.WriteLine(
                        $"Ç : {lesson.ClassName} - {lesson.Name} - {lesson.TeacherName} - {point.X} - {point.Y} | {item.ClassName} - {item.Name} - {item.TeacherName}");
                }

                Console.ResetColor();


                if (lesson.Teachers.Count > 1)
                {
                    var currentLesson = point.Lessons.First(x => x.TeacherName == lesson.TeacherName);

                    lesson.ChangeTeacherExtractWith(currentLesson.TeacherName);
                    return SetLesson(point, lesson);
                }

                this.MarkUnUsed(lesson)
                    .IncreaseRate();

                //Uncomment olduktan sonra ornek olarak tarih dersini sürekli alıp loop'a giriyor. multi enqueu yapıyor. O yuzden loop'a giriyor
                //ama yapmaz isek ders yutuyor bu logic'i tam oturtamadım çok kafam karıştı tüm case'leri handle etmek için 
                //çok vakit gitti sonra temiz kafayla buna bak çünkü ders yutmasa mantık dogru gibi yuttugu için queue'da ders olmayınca 
                //eksik kalıyor program ama conflict oldugunda kalanı queue'ya tekrar atması ve tekrar etmemezi lazım
                //yukardaki logic ise bu kontrol ederken bu seferde queue'dan bitsede çıkmıyor. ):
                //aslında bu method recursive olarak da düzgün çalışsa ondan sonra safe bir sonuc verir ama kod bozuk
                //oldugu için başka sonuctan veya çözümlerimin işe yaradıgından emin olamıyorum.
                // bu kodun recursive veya başka bir şekilde dogru queue'dan alıp kalanı atıp sonra bunu tekrar etmeden immutable olarak temiz 
                // çalışması lazım. Çok zaman harcadık problemin başka case'leri ile kafanı topla buraya tekrar dön.
                
                //this.EnqueueIfRemained(lesson);

                //bunu ayrı listeden alsak bu sefer o liste dolu oldugu sürece kullanılmasa bile diger listeye geçmez stack gibi çalışır
                // bu call edilirse ve ok olursa o zaman önce kinin state'ini kaybetmiş oluruz
                //true'yu bulana kadar calıstıgı için sonucu false gelmez sonucuna gore logic kurmamak lazım.
                // 3c friday resim dersi patlıyor.Son 2 saatin ilk saatini pas geçtiği için son dersin upper'ı olamaz oyuzden patlıyor.
                //recursive ve dynamic programming'in inceliklerini ogren
                var newLesson = DequeueWithCalculatedHour();
                if (newLesson == null) return false;
                SetLesson(point, newLesson);
                var hour = newLesson.Hour;
                var isUsed = this.EnqueueIfRemained(newLesson);

                //point.Upper != null aslında hour hesaplanarak geliyor ama çakışma oldugundan boyle oldu !!
                if (isUsed && hour > 1 && point.Upper != null)
                {
                    SetLesson(point.Upper, newLesson);
                }

                return true;
            }


            point.AddLesson(lesson
                .SetClassName(this.Name));

            return true;
        }


        /// <summary>
        /// TODO: 2 saatlik dersler parçalandı :(
        /// Tekli sayılardan alırsam aslında 2-2 'lik dersleri bölmem ama suan gerek yok
        /// Queue olarak olmadı !
        /// Rate olmadı !
        /// Random olmadı !
        /// Dersleri Shuffle ettim girdi olarak verilirken o da olmadı !
        /// Ama subelerin birleştirerek de çözüm önerimi literaturde var ?
        ///
        /// Literatur
        /// Graph Boyama
        /// Tabu search
        /// Tam sayı
        /// Benzetim Tavlaması (Simulated Annealing-SA)
        /// Np - hard problem bu problemin girdiği gruptur. Buna timetabling de denir.
        /// N-Queen ->https://www.geeksforgeeks.org/n-queen-problem-backtracking-3/#:~:text=The%20N%20Queen%20is%20the,blocks%20where%20queens%20are%20placed.
        /// Ama bunun time complexity'si çok fazla
        /// Kaynak : https://bilalsaim.com/yapay-zeka/sezgisel-optimizasyon-algoritmalari-heuristic-algorithms/
        ///
        /// Aslında çalıştıgım tüm şirketlerde servis scheduling problemi var . Acaba yapıp idari işlere satsak mı :) 
        /// </summary>
        /// <returns></returns>
        private Lesson DequeueWithCalculatedHour()
        {
            var lessons = Lessons.OrderBy(x => x.Rate).ToArray();
            var selectedLesson = lessons.FirstOrDefault();
            if (selectedLesson == null) return null;
            Lessons.Remove(selectedLesson);

            var hour = selectedLesson.Hour switch
            {
                0 => 2,
                1 => 1,
                _ => 2
            };

            selectedLesson.SetHour(hour);
            var remainedHours = Math.Abs(this.CurrentDailyHours - this.TotalDailyHours);
            if (remainedHours < selectedLesson.Hour && remainedHours != 0)
                selectedLesson.SetHour(remainedHours);

            this.CurrentDailyHours += selectedLesson.Hour;
            return selectedLesson;
        }

        private bool EnqueueIfRemained(Lesson lesson)
        {
            bool isUsed = lesson.Hour > 0;
            lesson.SumUsed(lesson.Hour);
            if (lesson.Used == lesson.TotalHour) return true;
            lesson.SetHour(lesson.TotalHour - lesson.Used);
            Lessons.Add(lesson);

            return isUsed;
        }

        private Lesson MarkUnUsed(Lesson lesson)
        {
            this.CurrentDailyHours -= lesson.Hour;
            lesson.SetHour(0);
            return lesson;
        }
    }
}