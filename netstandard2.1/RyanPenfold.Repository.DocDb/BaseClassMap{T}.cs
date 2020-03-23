using System;
using System.Collections.Generic;
using System.Text;
using RyanPenfold.BusinessBase.Infrastructure;

namespace RyanPenfold.Repository.DocDb
{
    /// <inheritdoc />
    public class BaseClassMap<T> : IClassMap
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BaseClassMap{T}"/> type.
        /// </summary>
        public BaseClassMap()
        {
            PrimaryKeyColumnNames = new List<string>();
        }

        /// <inheritdoc />
        public IList<string> PrimaryKeyColumnNames { get; set; }

        /// <inheritdoc />
        public string SchemaName { get; set; }

        /// <inheritdoc />
        public string TableName { get; set; }

        /// <summary>
        /// Derives a filename for the entity type <see cref="T"/>, based on the <see cref="SchemaName"/> and <see cref="TableName"/> property values.
        /// </summary>
        /// <returns>A file name</returns>
        public string DeriveFileName()
        {
            var rtnBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(SchemaName))
            {
                rtnBuilder.Append("[");
                rtnBuilder.Append(SchemaName);
                rtnBuilder.Append("]");
            }

            if (!string.IsNullOrWhiteSpace(TableName))
            {
                if (rtnBuilder.Length > 0)
                    rtnBuilder.Append(".");

                rtnBuilder.Append("[");
                rtnBuilder.Append(TableName);
                rtnBuilder.Append("]");
            }

            if (rtnBuilder.Length == 0)
                throw new InvalidOperationException("Impossible to derive file name.");

            rtnBuilder.Append(".json");

            return rtnBuilder.ToString();
        }
    }
}
