using System.IO;
using Backups.Tools;

namespace Backups.Models
{
    public class Repository
    {
        public Repository(string path)
        {
            Path = path ?? throw new BackupsException(
                "Invalid path of repository");
            Create();
        }

        public string Path { get; }

        public void Create()
        {
            if (Path != string.Empty)
            {
                Directory.CreateDirectory(Path);
            }
        }
    }
}
