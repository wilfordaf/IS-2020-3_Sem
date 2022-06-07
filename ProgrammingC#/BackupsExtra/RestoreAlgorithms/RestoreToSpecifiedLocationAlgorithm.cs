using System.IO;
using System.IO.Compression;
using Backups.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RestoreAlgorithms
{
    public class RestoreToSpecifiedLocationAlgorithm : IRestoreAlgorithm
    {
        private readonly string _specifiedPath;
        private readonly RestorePoint _restorePoint;

        public RestoreToSpecifiedLocationAlgorithm(string specifiedPath, RestorePoint restorePoint)
        {
            _specifiedPath = specifiedPath;
            _restorePoint = restorePoint;
        }

        public void Restore()
        {
            CheckExceptions();

            if (!Directory.Exists(_specifiedPath))
            {
                Directory.CreateDirectory(_specifiedPath);
            }

            foreach (string fileName in Directory.GetFiles(_restorePoint.Repository.Path))
            {
                if (fileName.Length < 6)
                {
                    continue;
                }

                if (fileName[^6..] != $"_{_restorePoint.NumberOfPoint}.zip")
                {
                    continue;
                }

                using ZipArchive storage = ZipFile.Open(fileName, ZipArchiveMode.Update);
                {
                    foreach (ZipArchiveEntry storageEntry in storage.Entries)
                    {
                        storageEntry.ExtractToFile($"{_specifiedPath}\\{storageEntry.Name}", true);
                    }
                }
            }
        }

        private void CheckExceptions()
        {
            _restorePoint.Objects.ForEach(o =>
            {
                if (!Directory.Exists(Path.GetDirectoryName(o.Path)))
                {
                    throw new BackupsExtraException();
                }
            });
        }
    }
}