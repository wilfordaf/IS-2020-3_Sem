using System;
using NUnit.Framework;
using Reports.API.Models.Accounts;
using Reports.API.Models.Members;
using Reports.API.Models.Reports;
using Reports.API.Models.Tasks;
using Reports.API.Models.Team;
using Reports.API.Tools;

namespace Reports.Tests
{
    public class Tests
    {
        private Team _team;

        [SetUp]
        public void SetUp()
        {
            _team = new Team();
        }

        [Test]
        public void AddTeamLeadAddMembersPrintHierarchySolveSomeTasksPrintSprintReport()
        {
            var t = new TeamLeadMember("abc");
            TeamLeadAccount ta = _team.SetTeamLeadAccount(t, "123", "123");
            var s1 = new StaffMember("abd", t.Id.ToString());
            var s2 = new StaffMember("dba", t.Id.ToString());
            var t1 = new Task("fff");
            var t2 = new Task("ggg");
            StaffMemberAccount a1 = _team.AddStaffMemberAccount(s1, "124", "123");
            StaffMemberAccount a2 = _team.AddStaffMemberAccount(s2, "125", "123");
            Console.WriteLine(_team.PrintAllMembers());
            a1.LogIn();
            a1.AddTask(t1);
            a1.LogOut();
            a2.LogIn();
            a2.AddTask(t2);
            a2.LogOut();
            _team.TaskManager.ChangeTaskEssense("aaa", t1);
            Assert.AreEqual("aaa", t1.Essense);
            a1.LogIn();
            a1.CompleteTask(t1);
            a1.Owner.SprintReport.Save();
            a1.LogOut();
            a2.LogIn();
            s2.SprintReport.Save();
            a2.LogOut();
            ta.LogIn();
            SprintReport s = _team.InitializeReportFormation();
            s.Data.ForEach(l => Console.WriteLine(l));
            ta.LogOut();
        }

        [Test]
        public void OneOfMembersForgotToSaveReport_ThrowException()
        {
            var t = new TeamLeadMember("abc");
            _team.SetTeamLeadAccount(t, "123", "123");
            var s1 = new StaffMember("abd", t.Id.ToString());
            _team.AddStaffMemberAccount(s1, "124", "123");
            Assert.Catch<ReportsException>(() =>
            {
                SprintReport s = _team.InitializeReportFormation();
            });
        }

        [Test]
        public void CreateAccountsWithSameLogin_ThrowException()
        {
            var t = new TeamLeadMember("abc");
            _team.SetTeamLeadAccount(t, "123", "123");
            Assert.Catch<ReportsException>(() =>
            {
                var s1 = new StaffMember("abd", t.Id.ToString());
                _team.AddStaffMemberAccount(s1, "123", "123");
            });
        }

        [Test]
        public void TryingToUseAccountFunctionsBeforeLoggingIn_ThrowException()
        {
            var t = new TeamLeadMember("abc");
            _team.SetTeamLeadAccount(t, "123", "123");
            var s1 = new StaffMember("abd", t.Id.ToString());
            var t1 = new Task("fff");
            StaffMemberAccount a1 = _team.AddStaffMemberAccount(s1, "124", "123");
            Assert.Catch<ReportsException>(() =>
            {
                a1.AddTask(t1);
            });
        }
    }
}