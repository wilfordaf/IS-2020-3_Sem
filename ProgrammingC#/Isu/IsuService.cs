using System.Collections.Generic;
using System.Linq;
using Isu.Services;

namespace Isu
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new List<Group>();

        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name);
            group.AddStudent(newStudent);
            newStudent.GroupNumber = group.Name;
            return newStudent;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            Group oldGroup = FindGroup(student.GroupNumber);
            AddStudent(newGroup, student.Name);
            oldGroup.DeleteStudent(student);
        }

        public Group FindGroup(string groupName)
        {
            return _groups.FirstOrDefault(g => g.Name == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(g => g.Course == courseNumber.Number).ToList();
        }

        public Student FindStudent(string name)
        {
            Group group = _groups.First(g => g.Students.FirstOrDefault(s => s.Name == name) != null);
            return group.Students.FirstOrDefault(s => s.Name == name);
        }

        public List<Student> FindStudents(string groupName)
        {
            return _groups.First(g => g.Name == groupName).Students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var course = new List<Student>();
            foreach (Group group in _groups)
            {
                course.AddRange(group.Students.Where(s => s.Course == courseNumber.Number));
            }

            return course.Any() ? course : null;
        }

        public Student GetStudent(int id)
        {
            Group group = _groups.First(g => g.Students.FirstOrDefault(s => s.Id == id) != null);
            return group.Students.FirstOrDefault(s => s.Id == id);
        }
    }
}