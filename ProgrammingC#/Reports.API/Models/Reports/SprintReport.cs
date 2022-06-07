using System.Collections.Generic;
using Reports.API.Models.Members;
using Reports.API.Tools;

namespace Reports.API.Models.Reports
{
    public class SprintReport
    {
        public List<StaffMember> MembersDoneSomething { get; set; }

        public List<StaffMember> MembersHaveNotDoneAnything { get; set; }

        public List<string> Data { get; set; } = new ();

        public SprintReport SetMembersDoneSomething(List<StaffMember> members)
        {
            MembersDoneSomething = members;
            return this;
        }

        public SprintReport SetMembersHaveNotDoneAnything(List<StaffMember> members)
        {
            MembersHaveNotDoneAnything = members;
            return this;
        }

        public SprintReport FormatReport()
        {
            if (MembersDoneSomething == null || MembersHaveNotDoneAnything == null)
            {
                throw new ReportsException("Can not format report before all values assigned");
            }

            MembersDoneSomething.ForEach(m =>
            {
                Data.Add($"Work done by member {m.Id}:");
                m.SprintReport.ActionsDone.ForEach(a => Data.Add(a));
            });

            MembersHaveNotDoneAnything.ForEach(m =>
            {
                Data.Add($"Member {m.Id} have not done anything");
            });

            return this;
        }
    }
}