using System;
using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class GroupExtra
    {
        private readonly Group _oldGroup;

        public GroupExtra(string name)
        {
            if (NameIsNotCorrect(name))
            {
                throw new IsuExtraException("Group name is not correct, follow the template LCXXX, L - Litera, C - course");
            }

            _oldGroup = new Group()
            {
                Name = name,
            };

            MFaculty = ResolveMFaculty(name[0]);
        }

        public string Name => _oldGroup.Name;

        public MFaculties MFaculty { get; }

        public Timetable Lessons { get; } = new ();

        public static MFaculties ResolveMFaculty(char letter)
        {
            switch (letter)
            {
                case 'M':
                case 'K':
                    return MFaculties.Tit;

                case 'A':
                case 'B':
                    return MFaculties.Bcs;

                case 'Z':
                case 'X':
                    return MFaculties.Ctc;

                case 'C':
                case 'F':
                    return MFaculties.Pe;

                case 'L':
                case 'N':
                    return MFaculties.Tmi;

                default:
                    throw new IsuExtraException("There is no MFaculty corresponding with " + letter);
            }
        }

        public void AddStudent(StudentExtra student)
        {
            _oldGroup.AddStudent(student);
        }

        public void DeleteStudent(StudentExtra student)
        {
            _oldGroup.DeleteStudent(student);
        }

        public void AddLesson(Lesson lesson)
        {
            Lessons.AddLesson(lesson);
        }

        public void DeleteLesson(Lesson lesson)
        {
            Lessons.DeleteLesson(lesson);
        }

        public bool HasIntersectionTimetable(Timetable newLessons)
        {
            return Lessons.HasIntersectionTimetable(newLessons);
        }

        public List<StudentExtra> StudentsWithNoOgnp()
        {
            return _oldGroup.Students.Where(s =>
                ((StudentExtra)s).OgnpAmount == 0).
                Cast<StudentExtra>().ToList();
        }

        private static bool NameIsNotCorrect(string name)
        {
            return name.Length != 5 ||
                   !char.IsLetter(name[0]) ||
                   !uint.TryParse(name[1..], out _) ||
                   Convert.ToInt32(name[1].ToString()) < 1 ||
                   Convert.ToInt32(name[1].ToString()) > 4;
        }
    }
}
