using System.IO;
using System.IO.Compression;
using Backups.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RestoreAlgorithms
{
    public class RestoreToOriginalLocationAlgorithm : IRestoreAlgorithm
    {
        private readonly RestorePoint _restorePoint;

        public RestoreToOriginalLocationAlgorithm(RestorePoint restorePoint)
        {
            _restorePoint = restorePoint;
        }

        public void Restore()
        {
            CheckExceptions();
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
                        _restorePoint.Objects.ForEach(o =>
                        {
                            if (o.Name() == storageEntry.Name)
                            {
                                storageEntry.ExtractToFile(o.Path, true);
                            }
                        });
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