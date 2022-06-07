using System.Collections.Generic;
using System.Linq;
using Backups.Algorithms;
using Backups.Tools;

namespace Backups.Models
{
    public class BackupJob<TAlgorithm>
        where TAlgorithm : class, IAlgorithm, new()
    {
        private readonly List<JobObject> _objects = new ();
        private readonly Repository _repository;

        public BackupJob()
            : this(string.Empty)
        {
        }

        public BackupJob(string repositoryName)
        {
            _repository = new Repository(repositoryName);
        }

        public List<RestorePoint> RestorePoints { get; set; } = new ();

        public int AmountOfRestorePoints => RestorePoints.Count;

        public JobObject AddJobObject(string path)
        {
            JobObject newJobObject = new (path);
            _objects.Add(newJobObject);
            return newJobObject;
        }

        public JobObject FindJobObject(string path)
        {
            return _objects.FirstOrDefault(f => f.Path == path);
        }

        public void DeleteJobObject(string path)
        {
            JobObject jobObjectFound = FindJobObject(path);
            _objects.Remove(jobObjectFound ?? throw new BackupsException(
                "You are trying to delete job object that does not exist"));
        }

        public RestorePoint AddRestorePoint()
        {
            var newRestorePoint = new RestorePoint(_objects, AmountOfRestorePoints, new TAlgorithm());
            newRestorePoint.Create(_repository);
            RestorePoints.Add(newRestorePoint);
            return newRestorePoint;
        }
    }
}
