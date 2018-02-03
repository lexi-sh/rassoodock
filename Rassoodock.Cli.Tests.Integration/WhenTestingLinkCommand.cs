using System;
using System.IO;
using Rassoodock.Tests.Base;
using Shouldly;
using Xunit;

namespace Rassoodock.Cli.Tests
{
    public class WhenTestingLinkCommand
    {
        [Fact]
        public void ShouldCreateAFolder()
        {
            var command = new LinkCommand();
            var databaseName = EnhancedRandom.String(10, 20);
            command.CreateLinkedDatabaseFolder(databaseName);
            var directory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(directory, $"{databaseName}.json");
            var folderName = Path.Combine(directory, databaseName);
            
            Directory.Exists(folderName).ShouldBe(true);
            File.Exists(fileName).ShouldBe(true);
        }

        [Fact]
        public void ShouldOverwriteAFolder()
        {
            var command = new LinkCommand();
            var databaseName = EnhancedRandom.String(10, 20);
            command.CreateLinkedDatabaseFolder(databaseName);
            var directory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(directory, $"{databaseName}.json");
            var folderName = Path.Combine(directory, databaseName);

            Directory.Exists(folderName).ShouldBe(true);
            File.Exists(fileName).ShouldBe(true);
            
            command.CreateLinkedDatabaseFolder(databaseName);
            Directory.Exists(folderName).ShouldBe(true);
            File.Exists(fileName).ShouldBe(true);
        }
    }
}
