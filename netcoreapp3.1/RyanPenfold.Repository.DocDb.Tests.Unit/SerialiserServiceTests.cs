using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RyanPenfold.Repository.DocDb.Tests.Unit
{
    [TestClass]
    public class SerialiserServiceTests
    {
        [TestMethod]
        public void GetBytes_WithData_ConvertsCorrectly()
        {
            // Arrange
            var ctor = typeof(SerialiserService).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
            if (ctor == null)
                throw new MissingMethodException("SerialiserService", "ctor");
            var serialiserService = (SerialiserService)ctor.Invoke(new object[0]);
            var input = "The quick brown fox jumps over the lazy dog.";
            var expectedResult = System.Text.Encoding.Unicode.GetBytes(input);
            var getBytesMethod = typeof(SerialiserService).GetMethod("GetBytes", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getBytesMethod == null)
                throw new MissingMethodException("SerialiserService", "GetBytes");

            // Act
            var result = (byte[])getBytesMethod.Invoke(serialiserService, new object[] { input });

            // Assert
            for (int i = 0; i < expectedResult.Length; i++)
                Assert.AreEqual(expectedResult[i], result[i]);
        }

        [TestMethod]
        public void GetString_WithData_ConvertsCorrectly()
        {
            // Arrange
            var ctor = typeof(SerialiserService).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
            if (ctor == null)
                throw new MissingMethodException("SerialiserService", "ctor");
            var serialiserService = (SerialiserService)ctor.Invoke(new object[0]);
            var expectedResult = "The quick brown fox jumps over the lazy dog.";
            var input = System.Text.Encoding.Unicode.GetBytes(expectedResult);
            var getStringMethod = typeof(SerialiserService).GetMethod("GetString", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getStringMethod == null)
                throw new MissingMethodException("SerialiserService", "GetString");

            // Act
            var result = (string)getStringMethod.Invoke(serialiserService, new object[] { input });

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
