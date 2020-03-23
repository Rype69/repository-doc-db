using System.Reflection;

namespace RyanPenfold.Repository.DocDb
{
    public class Configuration
    {
        /// <summary>
        /// Configuration section data
        /// </summary>
        private static IConfigurationSettings _configurationSettings;

        /// <summary>
        /// Gets or sets the strongly typed configuration section data.
        /// </summary>
        public static IConfigurationSettings ConfigurationSettings
        {
            get
            {
                // TODO: Should this always get the latest from the file?
                if (_configurationSettings != null)
                    return _configurationSettings;

                var assemblyName = MethodBase.GetCurrentMethod()?.DeclaringType?.Assembly.GetName().Name;

                var possibleConfigurationSectionNames = new[] { assemblyName };

                var count = 0;
                do
                {
                    _configurationSettings = RyanPenfold.Configuration.SettingsProvider<ConfigurationSettings>.GetSection(possibleConfigurationSectionNames[count]);
                    count++;
                } while (_configurationSettings == null && count < possibleConfigurationSectionNames.Length);

                return _configurationSettings;
            }
        }
    }
}
