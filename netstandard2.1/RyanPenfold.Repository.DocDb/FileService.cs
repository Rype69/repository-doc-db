using System.Threading.Tasks;

namespace RyanPenfold.Repository.DocDb
{
    /// <summary>
    /// Performs IO operations on files.
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FileService"/> type.
        /// </summary>
        internal FileService()
        {
        }

        /// <summary>
        /// Reads some data from a file
        /// </summary>
        /// <param name="filePath">The path to a file to read</param>
        /// <returns>A <see cref="T:byte[]"/></returns>
        internal async Task<byte[]> Read(string filePath)
        {
            return await System.IO.File.ReadAllBytesAsync(filePath);
        }

        /// <summary>
        /// Writes some data to a file
        /// </summary>
        /// <param name="filePath">The path to a file to write to</param>
        /// <param name="data">Some data to write to the file</param>
        internal async void Write(string filePath, byte[] data)
        {
            await System.IO.File.WriteAllBytesAsync(filePath, data);
        }
    }
}
