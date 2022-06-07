using System;
using System.Collections.Generic;
using Backups.Algorithms;
using Backups.Models;
using BackupsExtra.Models;
using NUnit.Framework;
using System.IO;
using System.Threading;
using BackupsExtra.ClearAlgorithms;
using BackupsExtra.Loggers;

namespace BackupsExtra.Tests
{
    [Ignore("Only local tests, but lab was already accepted by teachers")]
    public class BackupsExtraTests
    {
        private BackupJobExtra<SplitStoragesAlgorithm> _backupJobExtra;

        [Test]
        public void CreateTwoRestorePointsMergeThem()
        {
            var consoleLogger = new ConsoleLogger(true);

            var clearByAmountAlgorithm = new ClearByAmountAlgorithm(2);

            _backupJobExtra = new BackupJobExtra<SplitStoragesAlgorithm>(
                @"D:\Test",
                consoleLogger,
                clearByAmountAlgorithm);

            File.Create(@"D:\Test\123.txt").Close();
            File.Create(@"D:\Test\321.txt").Close();
            File.Create(@"D:\Test\213.txt").Close();
            _backupJobExtra.AddJobObject(@"D:\Test\123.txt");
            _backupJobExtra.AddJobObject(@"D:\Test\321.txt");
            _backupJobExtra.AddJobObject(@"D:\Test\213.txt");
            RestorePoint r1 = _backupJobExtra.AddRestorePoint();

            _backupJobExtra.DeleteJobObject(@"D:\Test\213.txt");
            Thread.Sleep(5000);
            RestorePoint r2 = _backupJobExtra.AddRestorePoint();

            _backupJobExtra.MergeRestorePoint(r1, r2);

            FileAssert.DoesNotExist(@"D:\Test\123.txt_1.zip");
            FileAssert.DoesNotExist(@"D:\Test\321.txt_1.zip");
            FileAssert.Exists(@"D:\Test\123.txt_2.zip");
            FileAssert.Exists(@"D:\Test\321.txt_2.zip");
            FileAssert.Exists(@"D:\Test\213.txt_2.zip");

            File.Delete(@"D:\Test\123.txt");
            File.Delete(@"D:\Test\321.txt");
            File.Delete(@"D:\Test\213.txt");
            File.Delete(@"D:\Test\123.txt_2.zip");
            File.Delete(@"D:\Test\321.txt_2.zip");
            File.Delete(@"D:\Test\213.txt_2.zip");
        }

        [Test]
        public void ClearRestorePointByHybridAlgorythmDoesNotSatistyAllAndAny()
        {
            var consoleLogger = new ConsoleLogger(true);

            DateTime dateTime = DateTime.Now + TimeSpan.FromMinutes(10);
            var clearByDateAlgorithm = new ClearByDateAlgorithm(dateTime);
            var clearByAmountAlgorithm = new ClearByAmountAlgorithm(2);
            var hybridList = new List<IClearAlgorithm> { clearByDateAlgorithm, clearByAmountAlgorithm };
            var hybrid1 = new HybridClearAlgorithm(HybridClearAlgorithmTypes.DoesNotSatisfyAll, hybridList);
            var hybrid2 = new HybridClearAlgorithm(HybridClearAlgorithmTypes.DoesNotSatisfyAny, hybridList);

            _backupJobExtra = new BackupJobExtra<SplitStoragesAlgorithm>(
                @"D:\Test",
                consoleLogger,
                hybrid1);

            File.Create(@"D:\Test\123.txt").Close();
            File.Create(@"D:\Test\321.txt").Close();
            _backupJobExtra.AddJobObject(@"D:\Test\123.txt");
            _backupJobExtra.AddJobObject(@"D:\Test\321.txt");
            _backupJobExtra.AddRestorePoint();
            _backupJobExtra.AddRestorePoint();
            _backupJobExtra.AddRestorePoint();
            _backupJobExtra.ClearRestorePoints();
            FileAssert.Exists(@"D:\Test\123.txt_1.zip");
            FileAssert.Exists(@"D:\Test\123.txt_2.zip");
            FileAssert.Exists(@"D:\Test\123.txt_3.zip");

            _backupJobExtra.ClearAlgorithm = hybrid2;
            _backupJobExtra.ClearRestorePoints();

            FileAssert.Exists(@"D:\Test\123.txt_2.zip");
            FileAssert.Exists(@"D:\Test\123.txt_3.zip");

            File.Delete(@"D:\Test\123.txt");
            File.Delete(@"D:\Test\321.txt");
            File.Delete(@"D:\Test\123.txt_2.zip");
            File.Delete(@"D:\Test\321.txt_2.zip");
            File.Delete(@"D:\Test\123.txt_3.zip");
            File.Delete(@"D:\Test\321.txt_3.zip");
        }

        [Test]
        public void RestoreFilesToOriginalLocationAndSpecifiedLocation()
        {
            var consoleLogger = new ConsoleLogger(true);

            var clearByAmountAlgorithm = new ClearByAmountAlgorithm(2);

            _backupJobExtra = new BackupJobExtra<SplitStoragesAlgorithm>(
                @"D:\Test",
                consoleLogger,
                clearByAmountAlgorithm);

            File.Create(@"D:\Test\123.txt").Close();
            File.Create(@"D:\Test\321.txt").Close();
            _backupJobExtra.AddJobObject(@"D:\Test\123.txt");
            _backupJobExtra.AddJobObject(@"D:\Test\321.txt");
            _backupJobExtra.AddRestorePoint();
            File.Delete(@"D:\Test\123.txt");
            File.Delete(@"D:\Test\321.txt");
            _backupJobExtra.UpbackFromRestorePointToOriginalLocation(1);
            FileAssert.Exists(@"D:\Test\123.txt");
            FileAssert.Exists(@"D:\Test\321.txt");
            _backupJobExtra.UpbackFromRestorePointToSpecifiedLocation(1, @"D:\Test\Test1");
            FileAssert.Exists(@"D:\Test\Test1\123.txt");
            FileAssert.Exists(@"D:\Test\Test1\321.txt");
            File.Delete(@"D:\Test\123.txt");
            File.Delete(@"D:\Test\321.txt");
            File.Delete(@"D:\Test\123.txt_1.zip");
            File.Delete(@"D:\Test\321.txt_1.zip");
            Directory.Delete(@"D:\Test\Test1", true);
        }
    }
}