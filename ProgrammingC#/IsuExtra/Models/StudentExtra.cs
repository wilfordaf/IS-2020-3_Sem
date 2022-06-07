using System.Collections.Generic;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class StudentExtra : Student
    {
        private const int MaxOgnp = 2;
        private readonly Timetable _lessons = new ();

        public StudentExtra(string name)
        {
            Name = name;
        }

        public List<string> OgnpList { get; } = new ();
        public int OgnpAmount => OgnpList.Count;

        public void AddOgnp(string ognpName, Timetable lessons)
        {
            if (OgnpAmount == MaxOgnp)
            {
                throw new IsuExtraException("This student has already signed up for 2 Ognp courses.");
            }

            if (HasIntersectionTimetable(lessons))
            {
                throw new IsuExtraException("Students timetable has intersection with lessons of the course.");
            }

            _lessons.AddTimetable(lessons);
            OgnpList.Add(ognpName);
        }

        public void DeleteOgnp(string ognpName)
        {
            if (!OgnpList.Contains(ognpName))
            {
                throw new IsuExtraException("Ognp you're trying to delete is not found.");
            }

            OgnpList.Remove(ognpName);
        }

        public bool HasIntersectionTimetable(Timetable newLessons)
        {
            return _lessons.HasIntersectionTimetable(newLessons);
        }
    }
}
