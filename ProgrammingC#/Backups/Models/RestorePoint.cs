using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Algorithms;
using Backups.Tools;

namespace Backups.Models
{
    public class RestorePoint
    {
        public RestorePoint(List<JobObject> objects, int number, IAlgorithm algorithm)
        {
            Objects = new List<JobObject>();
            objects.ForEach(o => Objects.Add(o));
            StorageAlgorithm = algorithm;
            NumberOfPoint = number + 1;
        }

        public DateTime Time { get; private set; }

        public int NumberOfPoint { get; }

        public IAlgorithm StorageAlgorithm { get; init; }

        public List<JobObject> Objects { get; init; }

        public Repository Repository { get; private set; }

        public void Create(Repository repository)
        {
            CheckExceptions();
            Repository = repository;
            StorageAlgorithm.Create(Objects, Repository, NumberOfPoint);
            Time = DateTime.Now;
        }

        private void CheckExceptions()
        {
            if (!Objects.All(o => o.Exists()))
            {
                throw new BackupsException("One of job objects you are trying to reach does not exist");
            }
        }
    }
}
