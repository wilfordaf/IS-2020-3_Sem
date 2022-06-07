namespace Isu
{
    public class Student
    {
        public Student() { }

        public Student(string name)
        {
            Name = name;
        }

        public string GroupNumber { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }

        public int Course =>
            System.Convert.ToInt32(GroupNumber.Substring(2, 1));
    }
}
