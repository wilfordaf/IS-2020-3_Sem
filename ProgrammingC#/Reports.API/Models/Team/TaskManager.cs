using System;
using System.Collections.Generic;
using System.Linq;
using Reports.API.Models.Members;
using Reports.API.Models.Tasks;

namespace Reports.API.Models.Team
{
    public class TaskManager
    { 
        public List<Task> Tasks { get; set; } = new ();

        public Task FindTaskById(string id)
        {
            return Tasks.FirstOrDefault(t => t.Id.ToString() == id);
        }

        public List<Task> FindTasksByCreationTime(DateTime time)
        {
            return Tasks.Where(t => t.CreationTime == time).ToList();
        }

        public List<Task> FindTaskByLastChangeTime(DateTime time)
        {
            return Tasks.Where(t => t.LastChangeTime == time).ToList();
        }

        public List<Task> FindTasksMemberMadeChanges(string memberId)
        {
            return Tasks.Where(t => t.MemberMadeChanges(memberId)).ToList();
        }

        public List<Task> GetAllEmployeersTasksOfMember(StaffMember member)
        {
            List<Task> tasks = new ();
            foreach (StaffMember staffMember in member.Employeers)
            {
                staffMember.Tasks.ForEach(t =>
                {
                    if (!tasks.Contains(t))
                    {
                        tasks.Add(t);
                    }
                });
            }

            return tasks;
        }

        public List<Task> GetAllEmployeersTasksOfTeamLead(TeamLeadMember teamLeadMember)
        {
            List<Task> tasks = new();
            foreach (StaffMember staffMember in teamLeadMember.Employeers)
            {
                staffMember.Tasks.ForEach(t =>
                {
                    if (!tasks.Contains(t))
                    {
                        tasks.Add(t);
                    }
                });
            }

            return tasks;
        }

        public Task CreateTask(string essense)
        {
            var task = new Task(essense);
            Tasks.Add(task);
            return task;
        }

        public void ChangeTaskEssense(string newEssense, Task task)
        {
            task.Essense = newEssense;
        }

        public void ChangeTaskResponsibleMember(StaffMember responsibleMember, Task task)
        {
            task.ResponsibleMember = responsibleMember;
            responsibleMember.Tasks.Add(task);
        }
    }
}