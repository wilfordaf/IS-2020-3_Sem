using Backups.Algorithms;
using Backups.Models;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var backupJob = new BackupJob<SplitStoragesAlgorithm>(@"D:\ITMO\3 semester\ProgrammingC#\Test");
            backupJob.AddJobObject(@"D:\ITMO\3 semester\ProgrammingC#\123.txt");
            backupJob.AddJobObject(@"D:\ITMO\3 semester\ProgrammingC#\321.txt");
            backupJob.AddRestorePoint();
        }
    }
}
