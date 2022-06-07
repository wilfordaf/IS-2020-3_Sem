using System.IO;
using Backups.Algorithms;
using Backups.Models;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupsTests
    {
        [Test]
        public void AddTwoFilesCreateRestorePointDeleteOneAddRestorePoint()
        {
            File.Create("123.txt").Close();
            File.Create("321.txt").Close();
            var backupJob = new BackupJob<SplitStoragesAlgorithm>();
            backupJob.AddJobObject("123.txt");
            backupJob.AddJobObject("321.txt");
            backupJob.AddRestorePoint();
            backupJob.DeleteJobObject("123.txt");
            backupJob.AddRestorePoint();
            Assert.AreEqual(2, backupJob.AmountOfRestorePoints);
            FileAssert.Exists("123.txt_1.zip");
            FileAssert.Exists("321.txt_1.zip");
            FileAssert.Exists("321.txt_2.zip");
            File.Delete("123.txt");
            File.Delete("321.txt");
            File.Delete("123.txt_1.zip");
            File.Delete("321.txt_1.zip");
            File.Delete("321.txt_2.zip");
        }
    }
}