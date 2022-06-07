using System;

namespace Reports.API.Models.Tasks
{
    public class TaskChange
    {
        public TaskChange()
        {
            
        }

        public TaskChange(string essense, string authorId)
        {
            Essense = essense;
            AuthorId = authorId;
        }

        public DateTime Time { get; set; } = DateTime.Now;

        public string Essense { get; set; }

        public string AuthorId { get; set; }
    }
}