using System.Collections.Generic;
using System.Linq;

namespace IsuExtra.Models
{
    public class MFaculty
    {
        public MFaculty(Ognp ognp, MFaculties name, List<GroupExtra> groups)
        {
            Ognp = ognp;
            Name = name;
            Groups = groups;
        }

        public MFaculties Name { get; }

        public Ognp Ognp { get; set; }

        public List<GroupExtra> Groups { get; }
        public GroupExtra FindGroupByName(string name)
        {
            return Groups.FirstOrDefault(g => g.Name == name);
        }
    }
}
