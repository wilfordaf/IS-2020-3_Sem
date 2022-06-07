using System.Collections.Generic;
using Backups.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.ClearAlgorithms
{
    public class ClearByAmountAlgorithm : IClearAlgorithm
    {
        private readonly int _amountToSave;

        public ClearByAmountAlgorithm(int amountToSave)
        {
            _amountToSave = amountToSave;
        }

        public List<RestorePoint> Clear(List<RestorePoint> restorePoints)
        {
            if (_amountToSave >= restorePoints.Count)
            {
                return new List<RestorePoint>();
            }

            List<RestorePoint> restorePointsToDelete = restorePoints.GetRange(0, restorePoints.Count - _amountToSave);

            if (restorePoints.Count <= restorePointsToDelete.Count)
            {
                throw new BackupsExtraException("By specified criteria all restore point should be deleted");
            }

            return restorePointsToDelete;
        }
    }
}