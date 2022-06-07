using System;
using System.Collections.Generic;
using System.Threading;
using Backups.Algorithms;
using BackupsExtra.ClearAlgorithms;
using BackupsExtra.Loggers;
using BackupsExtra.Models;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            DateTime dateTime = DateTime.Now + TimeSpan.FromSeconds(3);
            var consoleLogger = new ConsoleLogger(true);

            var clearByAmountAlgorithm = new ClearByAmountAlgorithm(100);
            var clearByDateAlgorithm = new ClearByDateAlgorithm(dateTime);
            var hybridList = new List<IClearAlgorithm> { clearByDateAlgorithm, clearByAmountAlgorithm };
            var hybrid = new HybridClearAlgorithm(HybridClearAlgorithmTypes.DoesNotSatisfyAny, hybridList);

            var backupJobExtra = new BackupJobExtra<SplitStoragesAlgorithm>(
                @"D:\ITMO\3 semester\ProgrammingC#\Test",
                consoleLogger,
                hybrid);
            backupJobExtra.AddJobObject(@"D:\ITMO\3 semester\ProgrammingC#\123.txt");
            backupJobExtra.AddJobObject(@"D:\ITMO\3 semester\ProgrammingC#\321.txt");
            backupJobExtra.AddRestorePoint();
            Thread.Sleep(2000);
            backupJobExtra.AddRestorePoint();
            Thread.Sleep(2000);
            backupJobExtra.AddRestorePoint();
            backupJobExtra.ClearRestorePoints();
        }
    }
}
