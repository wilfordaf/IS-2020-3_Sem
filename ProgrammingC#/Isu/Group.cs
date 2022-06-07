using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private const int MaxStudentsPerGroup = 5;

        public Group() { }

        public Group(string name)
        {
            if (NameIsNotCorrect(name))
            {
                throw new IsuException("Invalid group name, follow the template M3XXX");
            }

            Name = name;
        }

        public string Name { get; init; }

        public List<Student> Students { get; } = new List<Student>();

        public int Course => System.Convert.ToInt32(Name.Substring(2, 1));

        public void AddStudent(Student student)
        {
            if (Students.Count == MaxStudentsPerGroup)
            {
                throw new IsuException("This group is already full, try another one");
            }

            Students.Add(student);
            student.GroupNumber = Name;
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
        }

        public bool HasStudent(Student student)
        {
            return Students.Contains(student);
        }

        private static bool NameIsNotCorrect(string name)
        {
            return name.Length != 5 || name.Substring(0, 2) != "M3" || !int.TryParse(name.Substring(2, 3), out _);
        }
    }
}
