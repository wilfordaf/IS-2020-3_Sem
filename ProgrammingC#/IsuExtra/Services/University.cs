using System.Collections.Generic;
using System.Linq;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class University
    {
        private readonly List<MFaculty> _mFaculties = new ();

        public University(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<StudentExtra> GetStudentsFromOgnpStream(string ognpName, string streamName)
        {
            MFaculty mFacultyFound = FindMFacultyWithOgnp(ognpName);
            if (mFacultyFound is null)
            {
                throw new IsuExtraException("There is no MFaculty with this ognp.");
            }

            Ognp ognpFound = mFacultyFound.Ognp;
            Stream streamFound = ognpFound.Streams.FirstOrDefault(s => s.Name == streamName);
            if (streamFound is null)
            {
                throw new IsuExtraException("There is no MFaculty with this ognp.");
            }

            return streamFound.Students;
        }

        public HashSet<Stream> GetStreams(string ognpName)
        {
            MFaculty mFacultyFound = FindMFacultyWithOgnp(ognpName);
            if (mFacultyFound is null)
            {
                throw new IsuExtraException("There is no MFaculty with this ognp.");
            }

            Ognp ognpFound = mFacultyFound.Ognp;

            return ognpFound.Streams;
        }

        public List<StudentExtra> StudentsWithNoOgnp(GroupExtra group)
        {
            return group.StudentsWithNoOgnp();
        }

        public void ChangeStudentGroup(StudentExtra student, GroupExtra newGroup)
        {
            if (student.HasIntersectionTimetable(newGroup.Lessons))
            {
                throw new IsuExtraException();
            }

            GroupExtra groupFound = FindMFacultyByName(GroupExtra.ResolveMFaculty(student.GroupNumber[0]))
                .FindGroupByName(student.GroupNumber);

            newGroup.AddStudent(student);
            groupFound.DeleteStudent(student);
        }

        public void AddMFaculty(MFaculty newMFaculty)
        {
            _mFaculties.Add(newMFaculty);
        }

        public MFaculty FindMFacultyByName(MFaculties name)
        {
            return _mFaculties.FirstOrDefault(f => f.Name == name);
        }

        public MFaculty FindMFacultyWithOgnp(string ognpName)
        {
            return _mFaculties.FirstOrDefault(f => f.Ognp.Name == ognpName);
        }

        public void AddStudentToOgnp(StudentExtra student, string ognpName, string streamName)
        {
             MFaculty mFacultyFound = FindMFacultyWithOgnp(ognpName);
             if (mFacultyFound is null)
             {
                 throw new IsuExtraException("There is no MFaculty with this ognp.");
             }

             Ognp ognpFound = mFacultyFound.Ognp;
             Stream streamFound = ognpFound.Streams.FirstOrDefault(s => s.Name == streamName);
             GroupExtra group = FindMFacultyByName(GroupExtra.ResolveMFaculty(student.GroupNumber[0])).FindGroupByName(student.GroupNumber);

             if (streamFound is null)
             {
                 throw new IsuExtraException("There is no MFaculty with this ognp.");
             }

             if (group.MFaculty == mFacultyFound.Name)
             {
                 throw new IsuExtraException("The ognp course you chose is provided by your faculty.");
             }

             if (group.HasIntersectionTimetable(streamFound.Lessons))
             {
                 throw new IsuExtraException("The ognp course you chose has intersection with your group lessons.");
             }

             if (student.HasIntersectionTimetable(streamFound.Lessons))
             {
                 throw new IsuExtraException("The ognp course you chose has intersection with your other ognp lessons.");
             }

             streamFound.AddStudent(student);
             student.AddOgnp(ognpName, streamFound.Lessons);
        }

        public void DeleteStudentFromOgnp(StudentExtra student, string ognpName)
        {
            MFaculty mFacultyFound = FindMFacultyWithOgnp(ognpName);

            if (mFacultyFound is null)
            {
                throw new IsuExtraException("There is no MFaculty with this ognp.");
            }

            Ognp ognpFound = mFacultyFound.Ognp;

            ognpFound.DeleteStudent(student);
            student.DeleteOgnp(ognpName);
        }
    }
}
