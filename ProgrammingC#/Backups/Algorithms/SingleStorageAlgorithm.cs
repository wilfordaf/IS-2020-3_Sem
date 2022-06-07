using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Models;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SingleStorageAlgorithm : IAlgorithm
    {
        public void Create(List<JobObject> jobObjects, Repository repository, int number)
        {
            var newStorage = new Storage($"Single_Storage_{number}", repository);

            if (File.Exists(newStorage.Path))
            {
                throw new BackupsException($"{newStorage.Path} already exists");
            }

            ZipArchive archive = ZipFile.Open(newStorage.Path, ZipArchiveMode.Create);

            if (!jobObjects.Any(o => o.Exists()))
            {
                throw new BackupsException("One of job objects you are trying to reach does not exist");
            }

            jobObjects.ForEach(o =>
                archive.CreateEntryFromFile(o.Path, o.Name()));

            archive.Dispose();
        }
    }
}