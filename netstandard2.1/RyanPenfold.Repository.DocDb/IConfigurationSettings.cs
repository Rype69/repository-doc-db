namespace RyanPenfold.Repository.DocDb
{
    public interface IConfigurationSettings
    {
        /// <summary>
        /// Gets or sets the path to the directory where the data files are kept
        /// </summary>
        string DataDirectoryPath { get; set; }
    }
}