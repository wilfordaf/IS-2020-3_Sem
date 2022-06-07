using System.Collections.Generic;
using Backups.Models;

namespace Backups.Algorithms
{
    public interface IAlgorithm
    {
        void Create(List<JobObject> jobObjects, Repository repository, int number);
    }
}