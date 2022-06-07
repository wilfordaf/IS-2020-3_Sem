using System;
using System.Collections.Generic;
using System.Linq;
using Reports.API.Models.Members;

namespace Reports.API.Models.Tasks
{
    public class Task
    {
        private StaffMember _responsibleMember;

        public Task()
        {
            
        }

        public Task(string essense)
        {
            Essense = essense;
            CreationTime = DateTime.Now;
        }

        public string Essense { get; set; }

        public List<TaskChange> Changes { get; set; } = new();

        public List<string> Comments { get; set; } = new();

        public DateTime CreationTime { get; set; }

        public StaffMember ResponsibleMember
        {
            get => _responsibleMember;
            set
            {
                _responsibleMember = value;
                Status = TaskStatusType.Active;
                Changes.Add(new TaskChange($"Appointed new responsible member {value.Name}", value.ChiefId.ToString()));
            }
        }

        public TaskStatusType Status { get; set; } = TaskStatusType.Open;

        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime LastChangeTime => Changes.Last().Time;

        public void AddComment(string message, string commentatorId)
        {
            Comments.Add(message);
            Changes.Add(new TaskChange($"Added comment {message}", commentatorId));
        }

        public bool MemberMadeChanges(string memberId)
        {
            return Changes.Any(c => c.AuthorId.Equals(memberId));
        }
    }
}