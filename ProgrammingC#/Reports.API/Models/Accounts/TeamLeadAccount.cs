using System;
using Reports.API.Models.Members;
using Reports.API.Models.Reports;
using Reports.API.Tools;

namespace Reports.API.Models.Accounts
{
    public class TeamLeadAccount
    {
        public TeamLeadAccount()
        {
            
        }

        public TeamLeadAccount(TeamLeadMember teamLeadMember)
        {
            Owner = teamLeadMember;
        }

        public LoginData LoginData { get; set; }

        public TeamLeadMember Owner { get; set; }

        public bool IsLoggedIn { get; set; }

        public Guid Id => Owner.Id;

        public void LogIn()
        {
            IsLoggedIn = true;
        }

        public void LogOut()
        {
            IsLoggedIn = false;
        }

        public void NotifyEmployeersNewSprint()
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }

            Owner.NotifyEmployeersNewSprint();
        }

        public SprintReport MakeSprintReport()
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }

            return Owner.MakeSprintReport();
        }
    }
}