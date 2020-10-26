using System.Linq;

namespace Scheduler
{
    public class Scheduler
    {
        private readonly Sheet _sheet;

        public Scheduler(Sheet sheet)
        {
            _sheet = sheet;
        }

        /// <summary>
        /// TODO:Neden lesson'ların degerli aynı incele ?
        /// Answer : Aynı lesson'lar queue ve point list kullanılıyor yeni instance almak gerekir.
        /// </summary>
        public void Draw()
        {
            if (!_sheet.ClassRooms.Any())
                return;

            foreach (var classRoom in _sheet.ClassRooms)
            {
                classRoom.Draw();
            }
        }
    }
}