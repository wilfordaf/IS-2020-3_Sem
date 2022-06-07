using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Algorithms;
using Backups.Models;
using BackupsExtra.ClearAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.RestoreAlgorithms;
using BackupsExtra.Tools;

namespace BackupsExtra.Models
{
    public class BackupJobExtra<TAlgorithm>
        where TAlgorithm : class, IAlgorithm, new()
    {
        private readonly BackupJob<TAlgorithm> _oldBackupJob;
        private readonly ILogger _logger;

        public BackupJobExtra(string repositoryName, ILogger logger, IClearAlgorithm clearAlgorithm)
        {
            _oldBackupJob = new BackupJob<TAlgorithm>(repositoryName);
            _logger = logger;
            ClearAlgorithm = clearAlgorithm;
        }

        public IClearAlgorithm ClearAlgorithm { get; set; }

        public int AmountOfRestorePoints => _oldBackupJob.AmountOfRestorePoints;

        public JobObject AddJobObject(string path)
        {
            JobObject newJobObject = _oldBackupJob.AddJobObject(path);
            _logger.WriteLog($"Added job object with path {path}");
            return newJobObject;
        }

        public JobObject FindJobObject(string path)
        {
            return _oldBackupJob.FindJobObject(path);
        }

        public void DeleteJobObject(string path)
        {
            _oldBackupJob.DeleteJobObject(path);
            _logger.WriteLog($"Removed job object with path {path}");
        }

        public RestorePoint AddRestorePoint()
        {
            RestorePoint newRestorePoint = _oldBackupJob.AddRestorePoint();
            _logger.WriteLog($"Created restore point with number {_oldBackupJob.AmountOfRestorePoints}");
            return newRestorePoint;
        }

        public void RemoveRestorePointFiles(RestorePoint restorePoint)
        {
            if (restorePoint.Repository.Path != string.Empty &&
                !Directory.Exists(restorePoint.Repository.Path))
            {
                throw new BackupsExtraException("Repository does not exist");
            }

            foreach (string fileName in Directory.GetFiles(restorePoint.Repository.Path))
            {
                if (fileName.Length < 6)
                {
                    continue;
                }

                if (fileName[^6..] != $"_{restorePoint.NumberOfPoint}.zip")
                {
                    continue;
                }

                File.Delete(fileName);
            }
        }

        public void ClearRestorePoints()
        {
            List<RestorePoint> restorePointsToDelele = ClearAlgorithm.Clear(_oldBackupJob.RestorePoints);
            restorePointsToDelele.ForEach(r =>
            {
                RemoveRestorePointFiles(r);
                _logger.WriteLog($"Restore point number {r.NumberOfPoint} was removed");
            });

            _oldBackupJob.RestorePoints = _oldBackupJob.
                RestorePoints.
                Except(restorePointsToDelele).
                ToList();
        }

        public void MergeRestorePoint(RestorePoint restorePoint, RestorePoint otherRestorePoint)
        {
            RestorePoint oldRestorePoint = restorePoint;
            RestorePoint newRestorePoint = otherRestorePoint;

            if (otherRestorePoint.Time < restorePoint.Time)
            {
                oldRestorePoint = otherRestorePoint;
                newRestorePoint = restorePoint;
            }

            if (oldRestorePoint.StorageAlgorithm is SplitStoragesAlgorithm)
            {
                oldRestorePoint.Objects.ForEach(o =>
                {
                    if (newRestorePoint.Objects.All(ob => ob.Name() != o.Name()))
                    {
                        newRestorePoint.Objects.Add(o);
                    }
                });
            }

            RemoveRestorePointFiles(oldRestorePoint);
            RemoveRestorePointFiles(newRestorePoint);
            _oldBackupJob.RestorePoints.Remove(oldRestorePoint);

            newRestorePoint.Create(newRestorePoint.Repository);
            _logger.WriteLog($"Restore points with numbers {restorePoint.NumberOfPoint}" +
                             $" and {otherRestorePoint.NumberOfPoint} were merged");
        }

        public void UpbackFromRestorePointToOriginalLocation(int numberOfRestorePoint)
        {
            RestorePoint restorePoint =
                _oldBackupJob.
                    RestorePoints.
                    FirstOrDefault(r => r.NumberOfPoint == numberOfRestorePoint);

            _ = restorePoint ?? throw new BackupsExtraException("Restore point with this number does not exist");

            var restoreToOriginalLocationAlgorithm = new RestoreToOriginalLocationAlgorithm(restorePoint);
            restoreToOriginalLocationAlgorithm.Restore();
            _logger.WriteLog($"Files were restored from restore point number {numberOfRestorePoint}");
        }

        public void UpbackFromRestorePointToSpecifiedLocation(int numberOfRestorePoint, string path)
        {
            RestorePoint restorePoint =
                _oldBackupJob.
                    RestorePoints.
                    FirstOrDefault(r => r.NumberOfPoint == numberOfRestorePoint);

            _ = restorePoint ?? throw new BackupsExtraException("Restore point with this number does not exist");

            var restoreToSpecifiedLocationAlgorithm = new RestoreToSpecifiedLocationAlgorithm(path, restorePoint);
            restoreToSpecifiedLocationAlgorithm.Restore();
            _logger.WriteLog($"Files were restored from restore point number {numberOfRestorePoint}");
        }
    }
}