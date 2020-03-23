using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RyanPenfold.Repository.DocDb.Tests.Unit
{
    /// <summary>
    /// Provides tests for the <see cref="FileService"/> class.
    /// </summary>
    [TestClass]
    public class FileServiceTests
    {
        /// <summary>
        /// Tests the <see cref="FileService.Read"/> method.
        /// </summary>
        [TestMethod]
        public void Read_DataPresent_ReadsFromFile()
        {
            // Arrange
            var ctor = typeof(FileService).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
            if (ctor == null)
                throw new MissingMethodException("FileService", "ctor");
            var fileService = (FileService)ctor.Invoke(new object[0]);
            var readMethod = typeof(FileService).GetMethod("Read", BindingFlags.NonPublic | BindingFlags.Instance);
            if (readMethod == null)
                throw new MissingMethodException("FileService", "Read");
            var applicationDirectory = Utilities.IO.UncPath.GetApplicationDirectory();
            var fileName = $"{Guid.NewGuid()}.txt";
            var filePath = System.IO.Path.Combine(applicationDirectory, fileName);
            var data = System.Text.Encoding.Unicode.GetBytes("The quick brown fox jumps over the lazy dog.");
            System.IO.File.WriteAllBytes(filePath, data);

            try
            {
                // Act
                var result = ((Task<byte[]>)readMethod.Invoke(fileService, new object[] { filePath })).Result;

                // Assert
                for (int i = 0; i < data.Length; i++)
                    Assert.AreEqual(data[i], result[i]);
            }
            finally
            {
                Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="FileService.Write"/> method.
        /// </summary>
        [TestMethod]
        public void Write_DataPresent_FileExists()
        {
            // Arrange
            var ctor = typeof(FileService).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
            if (ctor == null)
                throw new MissingMethodException("FileService", "ctor");
            var fileService = (FileService)ctor.Invoke(new object[0]);
            var writeMethod = typeof(FileService).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance);
            if (writeMethod == null)
                throw new MissingMethodException("FileService", "Write");
            var applicationDirectory = Utilities.IO.UncPath.GetApplicationDirectory();
            var fileName = $"{Guid.NewGuid()}.txt";
            var filePath = System.IO.Path.Combine(applicationDirectory, fileName);

            try
            {
                // Act
                writeMethod.Invoke(fileService, new object[] {filePath, new List<byte>().ToArray() });

                // Assert
                Assert.IsTrue(System.IO.File.Exists(filePath));
            }
            finally
            {
                Utilities.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Tests the <see cref="FileService.Write"/> method.
        /// </summary>
        [TestMethod]
        public void Write_DataPresent_FileContainsData()
        {
            // Arrange
            var ctor = typeof(FileService).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
            if (ctor == null)
                throw new MissingMethodException("FileService", "ctor");
            var fileService = (FileService)ctor.Invoke(new object[0]);
            var writeMethod = typeof(FileService).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance);
            if (writeMethod == null)
                throw new MissingMethodException("FileService", "Write");
            var applicationDirectory = Utilities.IO.UncPath.GetApplicationDirectory();
            var fileName = $"{Guid.NewGuid()}.txt";
            var filePath = System.IO.Path.Combine(applicationDirectory, fileName);
            var data = System.Text.Encoding.Unicode.GetBytes("The quick brown fox jumps over the lazy dog.");

            try
            {
                // Act
                writeMethod.Invoke(fileService, new object[] { filePath, data });

                // Assert
                Assert.IsTrue(System.IO.File.Exists(filePath));
                var readFromFile = System.IO.File.ReadAllBytes(filePath);
                for (var i = 0; i < data.Length; i++)
                    Assert.AreEqual(data[i], readFromFile[i]);
            }
            finally
            {
                Utilities.IO.File.Delete(filePath);
            }
        }
    }
}
