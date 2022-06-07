using System;
using System.Collections.Generic;
using System.Linq;
using Reports.API.Models.Reports;
using Reports.API.Tools;

namespace Reports.API.Models.Members
{
    public class TeamLeadMember
    {
        public TeamLeadMember()
        {
            
        }

        public TeamLeadMember(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<StaffMember> Employeers { get; set; } = new();

        public void NotifyEmployeersNewSprint()
        {
            Employeers.ForEach(e => e.UpdateSprint());
        }

        public SprintReport MakeSprintReport()
        {
            if (Employeers.Any(e => !e.SprintReport.IsSaved))
            {
                throw new ReportsException("Member did not save his report, cannot make up sprint one");
            }

            List<StaffMember> membersDoneSomething = new ();
            List<StaffMember> membersHaveNotDoneAnything = new ();

            foreach (StaffMember staffMember in Employeers)
            {
                if (staffMember.SprintReport.IsEmpty)
                {
                    membersHaveNotDoneAnything.Add(staffMember);
                }
                else
                {
                    membersDoneSomething.Add(staffMember);
                }
                staffMember.Employeers.ForEach(e =>
                {
                    if (e.SprintReport.IsEmpty)
                    {
                        membersHaveNotDoneAnything.Add(e);
                    }
                    else
                    {
                        membersDoneSomething.Add(e);
                    }
                    
                });

                
            }

            return new SprintReport().
                SetMembersDoneSomething(membersDoneSomething).
                SetMembersHaveNotDoneAnything(membersHaveNotDoneAnything).
                FormatReport();
        }
    }
}