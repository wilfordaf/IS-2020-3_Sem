using System.Collections.Generic;
using Backups.Models;

namespace BackupsExtra.ClearAlgorithms
{
    public interface IClearAlgorithm
    {
        List<RestorePoint> Clear(List<RestorePoint> restorePoints);
    }
}