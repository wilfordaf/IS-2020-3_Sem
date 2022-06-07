using System;
using Reports.API.Models.Members;
using Reports.API.Models.Tasks;
using Reports.API.Tools;

namespace Reports.API.Models.Accounts
{
    public class StaffMemberAccount
    {
        public StaffMemberAccount()
        {
            
        }

        public StaffMemberAccount(StaffMember staffMember)
        {
            Owner = staffMember;
        }

        public LoginData LoginData { get; set; }

        public StaffMember Owner { get; set; }

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

        public void AddTask(Task task)
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }
            Owner.AddTask(task);
        }

        public void EditTask(Task task, string newEssense)
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }

            Owner.EditTask(task, newEssense);
        }

        public void DeleteTask(Task task)
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }

            Owner.DeleteTask(task);
        }

        public void CompleteTask(Task task)
        {
            if (!IsLoggedIn)
            {
                throw new ReportsException("User is not logged in");
            }

            Owner.CompleteTask(task);
        }
    }
}