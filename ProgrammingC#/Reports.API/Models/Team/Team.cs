using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Reports.API.Models.Accounts;
using Reports.API.Models.Members;
using Reports.API.Models.Reports;
using Reports.API.Tools;

namespace Reports.API.Models.Team
{
    public class Team
    {
        private const string DatabasePath = "Reports.db";

        public TeamLeadAccount TeamLead { get; set; }

        public List<StaffMemberAccount> Staff { get; set; } = new ();

        public TaskManager TaskManager { get; set; } = new ();

        public bool HasMember(StaffMember staffMember) 
            => Staff.Any(s => s.Owner == staffMember);

        public StaffMemberAccount AddStaffMemberAccount(StaffMember staffMember, string login, string password)
        {
            if (HasMember(staffMember))
            {
                throw new ReportsException($"Member {staffMember.Id} is already in team");
            }

            if (Staff.Any(l => l.LoginData.Login == login) || TeamLead is not null && TeamLead.LoginData.Login == login)
            {
                throw new ReportsException($"Account with {login} already exists, please choose another one");
            }

            if (TeamLead is not null && staffMember.ChiefId == TeamLead.Id)
            {
                TeamLead.Owner.Employeers.Add(staffMember);
            }
            else
            {
                GetMemberById(staffMember.ChiefId.ToString()).Owner.AddEmployeer(staffMember);
            }

            var newAccount = new StaffMemberAccount(staffMember)
            {
                LoginData = new LoginData(login, password)
            };

            Staff.Add(newAccount);
            return newAccount;
        }

        public TeamLeadAccount SetTeamLeadAccount(TeamLeadMember teamLeadMember, string login, string password)
        {
            if (Staff.Any(l => l.LoginData.Login == login) || TeamLead is not null && TeamLead.LoginData.Login == login)
            {
                throw new ReportsException($"Account with {login} already exists, please choose another one");
            }

            var newAccount = new TeamLeadAccount(teamLeadMember)
            {
                LoginData = new LoginData(login, password)
            };

            TeamLead = newAccount;
            return newAccount;
        }

        public TeamLeadAccount GetTeamLeadAccount(string login, string password)
        {
            var inputData = new LoginData(login, password);
            if (TeamLead is not null && !TeamLead.LoginData.IsEqual(inputData))
            {
                throw new ReportsException("Account with this data does not exist");
            }

            return TeamLead;
        }

        public StaffMemberAccount GetStaffMemberAccount(string login, string password)
        {
            var inputData = new LoginData(login, password);
            return Staff.FirstOrDefault(s => s.LoginData.IsEqual(inputData)) 
                   ?? throw new ReportsException("Account with this data does not exist");
        }

        public void DeleteStaffMemberAccount(StaffMember staffMember, string login, string password)
        {
            if (!HasMember(staffMember))
            {
                throw new ReportsException($"Member {staffMember.Id} is not in team");
            }

            if (TeamLead is not null && staffMember.ChiefId == TeamLead.Id)
            {
                TeamLead.Owner.Employeers.Remove(staffMember);
            }
            else
            {
                GetMemberById(staffMember.ChiefId.ToString()).Owner.DeleteEmployeer(staffMember);
            }

            Staff.Remove(GetStaffMemberAccount(login, password));
        }

        public bool CheckValidityOfData(string login, string password)
        {
            var inputData = new LoginData(login, password);
            return Staff.Any(s => s.LoginData.IsEqual(inputData));
        }

        public SprintReport InitializeReportFormation()
        {
            return TeamLead.MakeSprintReport();
        }

        public string PrintAllMembers()
        {
            string result = $"TeamLead - {TeamLead.Id}\n";
            foreach (StaffMember staffMember in TeamLead.Owner.Employeers)
            {
                result += $"  Leader - {staffMember.Id}\n";
                staffMember.Employeers.ForEach(e =>
                {
                    result += $"    Member - {e.Id}\n";
                });
            }

            return result;
        }

        public StaffMemberAccount GetMemberById(string id)
        {
            StaffMemberAccount result = Staff.FirstOrDefault(s => s.Id.ToString() == id);
            return result ?? throw new ReportsException("Account with this id does not exist");
        }

        public static Team LoadFromDatabase()
        {
            return File.Exists(DatabasePath)
                ? JsonSerializer.Deserialize<Team>(File.ReadAllText(DatabasePath))
                : new Team();
        }

        public void SaveToDatabase()
        {
            File.WriteAllText(DatabasePath, JsonSerializer.Serialize(this));
        }
    }
}