using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Stream
    {
        private const int MaxStudents = 100;

        public Stream(string name)
        {
            Name = name;
        }

        public Timetable Lessons { get; } = new ();

        public List<StudentExtra> Students { get; } = new ();

        public int Places => MaxStudents - Students.Count;

        public string Name { get; }

        public void AddStudent(StudentExtra student)
        {
            if (Places == 0)
            {
                throw new IsuExtraException("There are no available places in this stream.");
            }

            Students.Add(student);
        }

        public bool HasStudent(StudentExtra student)
        {
            return Students.Contains(student);
        }

        public void DeleteStudent(StudentExtra student)
        {
            if (!HasStudent(student))
            {
                throw new IsuExtraException("Student you're trying to delete is not found.");
            }

            Students.Remove(student);
        }

        public void AddLesson(Lesson lesson)
        {
            Lessons.AddLesson(lesson);
        }

        public void DeleteLesson(Lesson lesson)
        {
            Lessons.DeleteLesson(lesson);
        }
    }
}
