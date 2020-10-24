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