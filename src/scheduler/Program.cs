using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sheet sht = new Sheet(0, 2);
            Sheet sht = new Sheet(4, 4);

            sht.AddClass(ClassRoom.Load("1A", 5).SetLessonList(GetAllLessons()).SetSheet(sht));
            sht.AddClass(ClassRoom.Load("2B", 5).SetLessonList(GetAllLessons()).SetSheet(sht));
            sht.AddClass(ClassRoom.Load("3C", 5).SetLessonList(GetAllLessons()).SetSheet(sht));
            sht.AddClass(ClassRoom.Load("4D", 5).SetLessonList(GetAllLessons()).SetSheet(sht));

            Scheduler scheduler = new Scheduler(sht);
            scheduler.Draw();
        }

        /// <summary>
        /// salih ve özlem hanım kendi derslerinin dısında ortak matematik dersleri verirse
        /// ve salih'in tr dersi ,özlem'in cografya dersi aynı pointerda olursa
        /// ve 2 hocanın ortak dersleri olan mat. bu pointer'a gelirse hoca isimleri
        /// sürekli digerinden devam eder ama hepsinin aynı pointer'a gelen farklı dersleri olacagından dead lock olur.
        /// </summary>
        /// <returns></returns>
        public static List<Lesson> GetAllLessons()
        {
            return new List<Lesson>
            {
                Lesson.Load("Tarih", 3, new[] {"sultan"}.ToList()),
                Lesson.Load("Coğrafya", 3, new[] {"esra"}.ToList()),
                Lesson.Load("Türkçe", 4, new[] {"salih", "özlem"}.ToList()),
                Lesson.Load("Matematik", 4, new[] {"özlem", "mehmet"}.ToList()),
                Lesson.Load("Beden", 3, new[] {"alper"}.ToList()),
                Lesson.Load("Resim", 2, new[] {"alper"}.ToList()),
                Lesson.Load("Fizik", 2, new[] {"esra"}.ToList()),
                Lesson.Load("Kimya", 2, new[] {"sultan"}.ToList()),
                Lesson.Load("Biyoloji", 2, new[] {"salih"}.ToList())
            };
        }
    }
}