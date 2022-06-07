using System;
using System.Collections.Generic;
using Reports.API.Models.Reports;
using Reports.API.Models.Tasks;
using Reports.API.Tools;

namespace Reports.API.Models.Members
{
    public class StaffMember
    {
        public StaffMember()
        {
            
        }

        public StaffMember(string name, string chiefId)
        {
            Name = name;
            ChiefId = Guid.Parse(chiefId);
        }

        public string Name { get; set; }

        public Guid ChiefId { get; set; }

        public List<StaffMember> Employeers { get; set; } = new();

        public List<Task> Tasks { get; set; } = new ();

        public bool HasEmployeer(StaffMember employeer) => Employeers.Contains(employeer);

        public bool HasTask(Task task) => Tasks.Contains(task);

        public Guid Id { get; set; } = Guid.NewGuid();

        public MemberReport SprintReport { get; set; } = new ();

        public void AddEmployeer(StaffMember newEmployeer)
        {
            if (HasEmployeer(newEmployeer))
            {
                throw new ReportsException($"{newEmployeer.Name} is already an employeer of {Name}");
            }

            Employeers.Add(newEmployeer);
        }

        public void DeleteEmployeer(StaffMember employeer)
        {
            if (!HasEmployeer(employeer))
            {
                throw new ReportsException($"{employeer.Name} is not an employeer of {Name}");
            }

            Employeers.Remove(employeer);
        }

        public void AddTask(Task newTask)
        {
            if (HasTask(newTask))
            {
                throw new ReportsException($"{Name} is already working on this task");
            }

            Tasks.Add(newTask);
        }

        public void EditTask(Task task, string newEssense)
        {
            if (!HasTask(task))
            {
                throw new ReportsException($"{Name} does not have access to task {task.Id}");
            }

            task.Essense = newEssense;
        }

        public void DeleteTask(Task task)
        {
            if (!HasTask(task))
            {
                throw new ReportsException($"{Name} does not have the task {task.Id}");
            }

            task.Status = TaskStatusType.Open;
            Tasks.Remove(task);
        }

        public void CompleteTask(Task task)
        {
            if (!HasTask(task))
            {
                throw new ReportsException($"{Name} does not have access to task {task.Id}");
            }

            task.Status = TaskStatusType.Resolved;
            Tasks.Remove(task);
            SprintReport.AddActionDone($"Resolved task {task.Id}");
        }

        public void UpdateSprint()
        {
            Employeers.ForEach(e => e.UpdateSprint());
            SprintReport.Clear();
        }
    }
}