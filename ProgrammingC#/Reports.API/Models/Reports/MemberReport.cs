using System.Collections.Generic;

namespace Reports.API.Models.Reports
{
    public class MemberReport
    {
        public MemberReport() { }

        public bool IsSaved { get; set; }

        public List<string> ActionsDone { get; set; } = new ();

        public bool IsEmpty => ActionsDone.Count == 0;
        public void AddActionDone(string action)
        {
            ActionsDone.Add(action);
        }

        public void Save()
        {
            IsSaved = true;
        }

        public void Clear()
        {
            ActionsDone.Clear();
            IsSaved = false;
        }
    }
}