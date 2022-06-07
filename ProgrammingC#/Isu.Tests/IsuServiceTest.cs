using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group testGroup = _isuService.AddGroup("M3205");
            Student testStudent = _isuService.AddStudent(testGroup, "Yurpalov Sergey");
            Assert.AreEqual("M3205", testStudent.GroupNumber);
            Assert.IsTrue(_isuService.FindGroup("M3205").HasStudent(testStudent));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group testGroup = _isuService.AddGroup("M3205");
                for (int i = 0; i < 6; i++)
                {
                    _isuService.AddStudent(testGroup, i.ToString());
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M3ABC");

            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group newGroup = _isuService.AddGroup("M3105");
                for (int i = 0; i < 5; i++)
                {
                    _isuService.AddStudent(newGroup, i.ToString());
                }
                Group oldGroup = _isuService.AddGroup("M3203");
                Student test = _isuService.AddStudent(oldGroup, "Test");
                _isuService.ChangeStudentGroup(test, newGroup);
            });
        }
    }
}