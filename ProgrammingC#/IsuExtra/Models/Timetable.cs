using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Timetable
    {
        private readonly List<List<Lesson>> _timetable = Enumerable.Repeat(new List<Lesson>(), 7).ToList();

        public void AddLesson(Lesson newLesson)
        {
            if (HasIntersection(newLesson))
            {
                throw new IsuExtraException("Lesson you're trying to add has intersection with timetable");
            }

            _timetable[(int)newLesson.Day].Add(newLesson);
        }

        public void DeleteLesson(Lesson lesson)
        {
            if (!_timetable[(int)lesson.Day].Contains(lesson))
            {
                throw new IsuExtraException("Lesson you're trying to delete is not found.");
            }

            _timetable[(int)lesson.Day].Remove(lesson);
        }

        public bool HasIntersection(Lesson lesson)
        {
            return _timetable[(int)lesson.Day].Any(l => l.HasIntersection(lesson));
        }

        public bool HasIntersectionTimetable(Timetable lessons)
        {
            foreach (int dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                foreach (Lesson lesson in lessons._timetable[dayOfWeek])
                {
                    if (HasIntersection(lesson))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddTimetable(Timetable lessons)
        {
            if (HasIntersectionTimetable(lessons))
            {
                throw new IsuExtraException("Lessons you're trying to add has intersection with timetable");
            }

            foreach (int dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                _timetable[dayOfWeek].AddRange(lessons._timetable[dayOfWeek]);
            }
        }
    }
}
