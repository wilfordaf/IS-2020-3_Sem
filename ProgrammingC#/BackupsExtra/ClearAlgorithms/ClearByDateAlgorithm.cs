using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.ClearAlgorithms
{
    public class ClearByDateAlgorithm : IClearAlgorithm
    {
        private readonly DateTime _time;

        public ClearByDateAlgorithm(DateTime time)
        {
            _time = time;
        }

        public List<RestorePoint> Clear(List<RestorePoint> restorePoints)
        {
            var restorePointsToDelete = restorePoints.Where(r => r.Time < _time).ToList();
            if (restorePointsToDelete.Count == restorePoints.Count)
            {
                throw new BackupsExtraException("By specified criteria all restore point should be deleted");
            }

            return restorePointsToDelete;
        }
    }
}