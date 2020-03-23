namespace RyanPenfold.Repository.DocDb
{
    /// <summary>
    /// Performs conversions between <see cref="T:byte[]"/> objects and <see cref="string"/> objects.
    /// </summary>
    public class SerialiserService
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SerialiserService"/> type.
        /// </summary>
        internal SerialiserService()
        {
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="data">A <see cref="string"/></param>
        /// <returns>A <see cref="T:byte[]"/>.</returns>
        internal byte[] GetBytes(string data)
        {
            return System.Text.Encoding.Unicode.GetBytes(data);
        }

        /// <summary>
        /// Converts a <see cref="T:byte[]"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="data">A <see cref="T:byte[]"/></param>
        /// <returns>A <see cref="string"/>.</returns>
        internal string GetString(byte[] data)
        {
            return System.Text.Encoding.Unicode.GetString(data);
        }
    }
}
