using System;
using System.Collections.Generic;
using NUnit.Framework;
using IsuExtra.Services;
using IsuExtra.Models;
using IsuExtra.Tools;


namespace IsuExtra.Tests
{
    public class Tests
    {
        private University _university;

        [SetUp]
        public void Setup()
        {
            _university = new University("ITMO");
        }

        [Test]
        public void AddStudentToOgnp_StudentsWithUnfilledOgnp_DeleteStudentFromOgnp()
        {
            var stream1 = new Stream("Healthy Programming 1");
            stream1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "05:36", "Wolf", "healthy sleep"));

            var stream2 = new Stream("Oxford residents 1");
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "15:10", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "17:00", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "18:40", "Andy", "English"));


            var streams1 = new HashSet<Stream> { stream1 };
            var streams2 = new HashSet<Stream> { stream2 };

            Ognp ognp1 = new("Healthy Programming", streams1);
            Ognp ognp2 = new("Oxford residents", streams2);

            StudentExtra student1 = new("Sergey");
            StudentExtra student2 = new("Manuchehrhon");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);
            group1.AddStudent(student2);
            group1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "10:00", "123", "123"));

            GroupExtra group2 = new("F2301");

            var groups1 = new List<GroupExtra> { group1 };
            var groups2 = new List<GroupExtra> { group2 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);
            MFaculty mFaculty2 = new(ognp2, MFaculties.Bcs, groups2);

            _university.AddMFaculty(mFaculty1);
            _university.AddMFaculty(mFaculty2); 
            

            _university.AddStudentToOgnp(student1, "Oxford residents", "Oxford residents 1");

            Assert.AreEqual(1, student1.OgnpAmount);
            Assert.AreEqual(ognp2.Name, student1.OgnpList[0]);

            var test = new List<StudentExtra>() {student2};
            Assert.AreEqual(test, group1.StudentsWithNoOgnp());

            _university.DeleteStudentFromOgnp(student1, "Oxford residents");
            Assert.AreEqual(0, student1.OgnpAmount);
            Assert.AreEqual(new List<string> (), student1.OgnpList);
        }

        [Test]
        public void UnableToAddOgnpIntersectionWithTimetable_ThrowException()
        {
            var stream1 = new Stream("Healthy Programming 1");
            stream1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "05:36", "Wolf", "healthy sleep"));

            var stream2 = new Stream("Oxford residents 1");
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "15:10", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "17:00", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "18:40", "Andy", "English"));


            var streams1 = new HashSet<Stream> { stream1 };
            var streams2 = new HashSet<Stream> { stream2 };

            Ognp ognp1 = new("Healthy Programming", streams1);
            Ognp ognp2 = new("Oxford residents", streams2);

            StudentExtra student1 = new("Sergey");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);
            group1.AddLesson(new Lesson(DayOfWeek.Friday, "123", "15:00", "123", "123"));

            GroupExtra group2 = new("F2301");

            var groups1 = new List<GroupExtra> { group1 };
            var groups2 = new List<GroupExtra> { group2 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);
            MFaculty mFaculty2 = new(ognp2, MFaculties.Bcs, groups2);

            _university.AddMFaculty(mFaculty1);
            _university.AddMFaculty(mFaculty2);

            Assert.Catch<IsuExtraException>(() =>
            {
                _university.AddStudentToOgnp(student1, "Oxford residents", "Oxford residents 1");
            });
        }

        [Test]
        public void UnableToAddOgnpMoreThanTwoOnStudent_ThrowException()
        {
            var stream1 = new Stream("Healthy Programming 1");
            var stream2 = new Stream("Oxford residents 1");
            var stream3 = new Stream("PSG Enjoyers 1");
            var stream4 = new Stream("Coders 1");

            stream1.AddLesson(new Lesson(DayOfWeek.Saturday, "321", "05:36", "Wolf", "healthy sleep"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "18:40", "Andy", "English"));
            stream3.AddLesson(new Lesson(DayOfWeek.Sunday, "666", "21:00", "Povish", "EVM"));
            stream4.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            
            var streams1 = new HashSet<Stream> {stream1};
            var streams2 = new HashSet<Stream> {stream2};
            var streams3 = new HashSet<Stream> {stream3};
            var streams4 = new HashSet<Stream> {stream4};

            Ognp ognp1 = new("Healthy Programming", streams1);
            Ognp ognp2 = new("Oxford residents", streams2);
            Ognp ognp3 = new("PSG Enjoyers", streams3);
            Ognp ognp4 = new("Coders", streams4);

            StudentExtra student1 = new("Sergey");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);

            GroupExtra group2 = new("F2301");
            GroupExtra group3 = new("A2301");
            GroupExtra group4 = new("Z2301");

            var groups1 = new List<GroupExtra> { group1 };
            var groups2 = new List<GroupExtra> { group2 };
            var groups3 = new List<GroupExtra> { group3 };
            var groups4 = new List<GroupExtra> { group4 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);
            MFaculty mFaculty2 = new(ognp2, MFaculties.Bcs, groups2);
            MFaculty mFaculty3 = new(ognp3, MFaculties.Pe, groups3);
            MFaculty mFaculty4 = new(ognp4, MFaculties.Ctc, groups4);

            _university.AddMFaculty(mFaculty1);
            _university.AddMFaculty(mFaculty2);
            _university.AddMFaculty(mFaculty3);
            _university.AddMFaculty(mFaculty4);

            Assert.Catch<IsuExtraException>(() =>
            {
                _university.AddStudentToOgnp(student1, "Oxford residents", " Oxford residents 1");
                _university.AddStudentToOgnp(student1, "Coders", "Coders 1");
                _university.AddStudentToOgnp(student1, "PSG Enjoyers", "PSG Enjoyers 1");
            });
        }

        [Test]
        public void UnableToAddOgnpOfStudentsOwnFaculty_ThrowException()
        {
            var stream1 = new Stream("Healthy Programming 1");
            stream1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "05:36", "Wolf", "healthy sleep"));

            var streams1 = new HashSet<Stream> { stream1 };

            Ognp ognp1 = new("Healthy Programming", streams1);

            StudentExtra student1 = new("Sergey");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);

            var groups1 = new List<GroupExtra> { group1 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);

            _university.AddMFaculty(mFaculty1);


            Assert.Catch<IsuExtraException>(() =>
            {
                _university.AddStudentToOgnp(student1, "Healthy Programming", "Healthy Programming 1");
            });
        }

        [Test]
        public void UnableToDeleteOgnpFromStudentWithoutThisOgnp_ThrowException()
        {
            var stream1 = new Stream("Healthy Programming 1");
            stream1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "05:36", "Wolf", "healthy sleep"));

            var stream2 = new Stream("Oxford residents 1");
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "15:10", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "17:00", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "18:40", "Andy", "English"));


            var streams1 = new HashSet<Stream> { stream1 };
            var streams2 = new HashSet<Stream> { stream2 };

            Ognp ognp1 = new("Healthy Programming", streams1);
            Ognp ognp2 = new("Oxford residents", streams2);

            StudentExtra student1 = new("Sergey");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);

            GroupExtra group2 = new("F2301");

            var groups1 = new List<GroupExtra> { group1 };
            var groups2 = new List<GroupExtra> { group2 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);
            MFaculty mFaculty2 = new(ognp2, MFaculties.Bcs, groups2);

            _university.AddMFaculty(mFaculty1);
            _university.AddMFaculty(mFaculty2);

            _university.AddStudentToOgnp(student1, "Oxford residents", "Oxford residents 1");

            Assert.Catch<IsuExtraException>(() =>
            {
                _university.DeleteStudentFromOgnp(student1, "Healthy Programming");
            });
        }

        [Test]
        public void CreateStreamWithIntercentionsInTimetable_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var stream1 = new Stream("Healthy Programming 1");
                stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "123", "11:40", "Wolf", "programming"));
                stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "11:36", "Wolf", "healthy sleep"));
            });
        }

        [Test]
        public void CreateStreamWithIncorrectTimeFormat_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var stream1 = new Stream("Healthy Programming 1");
                stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "123", "11 40", "Wolf", "programming"));
                stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "24:88", "Wolf", "healthy sleep"));
                stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "4:13", "Wolf", "BGD"));
            });
        }

        [Test]
        public void UnableToChangeStudentGroupHasIntersectionTimetable_ThrowException()
        {
            var stream1 = new Stream("Healthy Programming 1");
            stream1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "11:40", "Wolf", "programming"));
            stream1.AddLesson(new Lesson(DayOfWeek.Sunday, "321", "05:36", "Wolf", "healthy sleep"));

            var stream2 = new Stream("Oxford residents 1");
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "15:10", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "17:00", "Andy", "English"));
            stream2.AddLesson(new Lesson(DayOfWeek.Friday, "3321", "18:40", "Andy", "English"));

            var streams1 = new HashSet<Stream> { stream1 };
            var streams2 = new HashSet<Stream> { stream2 };

            Ognp ognp1 = new("Healthy Programming", streams1);
            Ognp ognp2 = new("Oxford residents", streams2);

            StudentExtra student1 = new("Sergey");
            StudentExtra student2 = new("Manuchehrhon");

            GroupExtra group1 = new("M3205");
            group1.AddStudent(student1);
            group1.AddStudent(student2);
            group1.AddLesson(new Lesson(DayOfWeek.Monday, "123", "10:00", "123", "123"));

            GroupExtra group2 = new("F2301");
            group2.AddLesson(new Lesson(DayOfWeek.Friday, "123", "15:00", "123", "123"));

            var groups1 = new List<GroupExtra> { group1 };
            var groups2 = new List<GroupExtra> { group2 };

            MFaculty mFaculty1 = new(ognp1, MFaculties.Tit, groups1);
            MFaculty mFaculty2 = new(ognp2, MFaculties.Bcs, groups2);

            _university.AddMFaculty(mFaculty1);
            _university.AddMFaculty(mFaculty2);


            _university.AddStudentToOgnp(student1, "Oxford residents", "Oxford residents 1");
           
            Assert.Catch<IsuExtraException>(() =>
            {
                _university.ChangeStudentGroup(student1, group2);
            });
        }
    }
}