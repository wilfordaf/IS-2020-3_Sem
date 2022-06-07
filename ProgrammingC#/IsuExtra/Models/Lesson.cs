using System;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Lesson
    {
        private const int HoursInDay = 24;
        private const int MinutesInHour = 60;
        private const int LessonDuration = 90;

        public Lesson(DayOfWeek day, string room, string time, string teacher, string name)
        {
            if (TimeIsNotValid(time))
            {
                throw new IsuExtraException("Time format is not correct, follow the template hh:mm.");
            }

            Name = name;
            Day = day;
            Room = room;
            Teacher = teacher;
            Time = time;
        }

        public DayOfWeek Day { get; }

        public string Room { get; }

        public string Teacher { get; }

        public string Time { get; }
        public string Name { get; }

        public int TimeToMinutes => (int.Parse(Time[..2]) * MinutesInHour) + int.Parse(Time[3..5]);

        public bool HasIntersection(Lesson lesson)
        {
            if (lesson.Day != Day)
            {
                return false;
            }

            int lessonTime = lesson.TimeToMinutes;
            int lowerBorderTime = TimeToMinutes - LessonDuration;
            int upperBorderTime = TimeToMinutes + LessonDuration;

            return lessonTime > lowerBorderTime && lessonTime < upperBorderTime;
        }

        private static bool TimeIsNotValid(string time)
        {
            return time.Length != 5 ||
                   !uint.TryParse(time[..2], out uint hours) ||
                   !uint.TryParse(time[3..5], out uint minutes) ||
                   time[2] != ':' ||
                   hours > HoursInDay ||
                   minutes >= MinutesInHour;
        }
    }
}
