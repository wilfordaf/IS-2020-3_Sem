using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Ognp
    {
        public Ognp(string name, HashSet<Stream> streams)
        {
            Name = name;
            Streams = streams;
        }

        public HashSet<Stream> Streams { get; }
        public int StreamsAmount => Streams.Count;
        public string Name { get; }

        public Stream FindStreamByStudent(StudentExtra student)
        {
            return Streams.FirstOrDefault(s => s.HasStudent(student));
        }

        public void DeleteStudent(StudentExtra student)
        {
            Stream streamFound = FindStreamByStudent(student);
            if (streamFound is null)
            {
                throw new IsuExtraException("Student you're trying to delete is not found.");
            }

            streamFound.DeleteStudent(student);
        }
    }
}
