using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Models;
using Backups.Tools;

namespace Backups.Algorithms
{
    public class SplitStoragesAlgorithm : IAlgorithm
    {
        public void Create(List<JobObject> jobObjects, Repository repository, int number)
        {
            if (!jobObjects.Any(o => o.Exists()))
            {
                throw new BackupsException("One of job objects you are trying to reach does not exist");
            }

            foreach (JobObject jobObject in jobObjects)
            {
                var newStorage = new Storage($"{jobObject.Name()}_{number}", repository);
                if (File.Exists(newStorage.Path))
                {
                    throw new BackupsException($"{newStorage.Path} already exists");
                }

                ZipArchive archive = ZipFile.Open(newStorage.Path, ZipArchiveMode.Create);
                archive.CreateEntryFromFile(jobObject.Path, jobObject.Name());

                archive.Dispose();
            }
        }
    }
}