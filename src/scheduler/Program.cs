﻿using System.Collections.Generic;

namespace LessonProgramSheet
{
    class Program
    {
        /// <summary>
        /// lesson's hours must be divide
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var lessons = GetAllLessons();
            
            Sheet sht = new Sheet(5, 5);
            sht.AddClass(ClassRoom.Load("1A").SetLessonList(lessons));
            sht.AddClass(ClassRoom.Load("2B").SetLessonList(lessons));
        }

        /// <summary>
        /// hocanın  sınıf ile ilişkisi
        /// ders gelir ve aynı dersten alındımı bakılır?
        /// eger alındıysa kotasına bakılır
        /// kota müsaitse gelen dersin hocası ile daha önce alınan dersin hocası aynı mı diye bakılır
        /// değilse pas geçilir. Aynı ise assign edilir.
        /// kota yoksa zaten o sınıf o dersi almıstır. Diger sınıf için alınabilir.
        /// Ayrıca ders kotaları tutulmalıdır ve her sınıf için ders listesi O(n) olarak
        /// search edililirse sıkntı olabilir ?
        ///
        /// ders ataması
        /// ders gelir
        /// önce derse bakılır ders aynı ise hocaya bakılır o da aynı ise bir sonraki pointe assign edelir. Çünkü o ders başka sınıf için verilmiştir
        /// ders aynı ama hoca farklı ise aynı pointe asign edilir. Burda o dersin kotasına bakılır o sınıf daha önce o dersi aldıysa aynı hocadan diger
        /// saatin verilmiş için 1. case'e gidilir.
        ///
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Lesson> GetAllLessons()
        {
            return new List<Lesson>
            {
                new Lesson()
                {
                    Name = "Tarih",
                    TotalHour = 3,
                    Teacher = new List<string>()
                    {
                        "Sultan"
                    }
                },

                new Lesson()
                {
                    Name = "Coğrafya",
                    TotalHour = 3,
                    Teacher = new List<string>()
                    {
                        "Esra"
                    }
                },

                new Lesson()
                {
                    Name = "Türkçe",
                    TotalHour = 4,
                    Teacher = new List<string>()
                    {
                        "Salih",
                        "Özlem"
                    }
                },

                new Lesson()
                {
                    Name = "Matematik",
                    TotalHour = 4,
                    Teacher = new List<string>()
                    {
                        "Özlem",
                        "Mehmet"
                    }
                },

                new Lesson()
                {
                    Name = "Beden",
                    TotalHour = 3,
                    Teacher = new List<string>()
                    {
                        "Alper",
                    }
                },
                new Lesson()
                {
                    Name = "Fizik",
                    TotalHour = 2,
                    Teacher = new List<string>()
                    {
                        "Esra"
                    }
                },
                new Lesson()
                {
                    Name = "Kimya",
                    TotalHour = 2,
                    Teacher = new List<string>()
                    {
                        "Sultan"
                    }
                },
                new Lesson()
                {
                    Name = "Resim",
                    TotalHour = 2,
                    Teacher = new List<string>()
                    {
                        "Alper",
                    }
                },
                new Lesson()
                {
                    Name = "Biyoloji",
                    TotalHour = 2,
                    Teacher = new List<string>()
                    {
                        "Salih"
                    }
                }
            };
        }
    }
}