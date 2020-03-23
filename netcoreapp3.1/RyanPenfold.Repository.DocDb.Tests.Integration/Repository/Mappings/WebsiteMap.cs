// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsiteMap.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using RyanPenfold.Repository.DocDb.Tests.Integration.Models;
using RyanPenfold.Utilities.Collections.Generic;

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Repository.Mappings
{
    /// <summary>
    /// Defines a mapping for type <see cref="Website"/>.
    /// </summary>
    public class WebsiteMap : BaseClassMap<Website>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteMap"/> class.
        /// </summary>
        public WebsiteMap()
        {
            this.TableName = "Website";
            this.SchemaName = "Foundation";
            this.PrimaryKeyColumnNames.UniqueAdd("Id");
        }
    }
}