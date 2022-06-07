using System.IO;
using Backups.Tools;

namespace Backups.Models
{
    public class JobObject
    {
        public JobObject(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsException("You are trying to create job object from file that does not exist");
            }

            Path = path;
        }

        public string Path { get; }

        public bool Exists()
        {
            return File.Exists(Path);
        }

        public string Name()
        {
            return Path.Split('\\')[^1];
        }
    }
}
