using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.ClearAlgorithms
{
    public class HybridClearAlgorithm : IClearAlgorithm
    {
        private readonly HybridClearAlgorithmTypes _clearType;
        private readonly List<IClearAlgorithm> _clearAlgorithms;

        public HybridClearAlgorithm(
            HybridClearAlgorithmTypes clearType,
            List<IClearAlgorithm> clearAlgorithms)
        {
            _clearType = clearType;
            _clearAlgorithms = clearAlgorithms;
        }

        public List<RestorePoint> Clear(List<RestorePoint> restorePoints)
        {
            switch (_clearType)
            {
                case HybridClearAlgorithmTypes.DoesNotSatisfyAny:
                    HashSet<RestorePoint> restorePointsToDeleteAny = new ();
                    _clearAlgorithms.ForEach(c => restorePointsToDeleteAny.UnionWith(c.Clear(restorePoints)));
                    return restorePointsToDeleteAny.ToList();

                case HybridClearAlgorithmTypes.DoesNotSatisfyAll:
                    List<RestorePoint> restorePointsToDeleteAll = new ();
                    foreach (RestorePoint restorePoint in restorePoints)
                    {
                        if (_clearAlgorithms.All(c => c.Clear(restorePoints).Contains(restorePoint)))
                        {
                            restorePointsToDeleteAll.Add(restorePoint);
                        }
                    }

                    return restorePointsToDeleteAll;
                default:
                    throw new BackupsExtraException("There is no such Hybrid Clear Algorithm type.");
            }
        }
    }
}