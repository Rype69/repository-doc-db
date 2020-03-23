using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RyanPenfold.Repository.DocDb.Tests.Integration
{
    [TestClass]
    public class AssemblyInitialise
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            RyanPenfold.Configuration.SettingsProvider.ConfigurationFileName = "appsettings.test.json";
        }
    }
}