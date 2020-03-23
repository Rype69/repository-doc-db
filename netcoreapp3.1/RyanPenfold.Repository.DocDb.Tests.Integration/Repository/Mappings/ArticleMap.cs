// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleMap.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using RyanPenfold.Repository.DocDb.Tests.Integration.Models;
using RyanPenfold.Utilities.Collections.Generic;

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Repository.Mappings
{
    /// <summary>
    /// Defines a mapping for type <see cref="Article"/>.
    /// </summary>
    public class ArticleMap : BaseClassMap<Article>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleMap"/> class.
        /// </summary>
        public ArticleMap()
        {
            this.TableName = "Article";
            this.SchemaName = "Foundation";
            this.PrimaryKeyColumnNames.UniqueAdd("Id");
        }
    }
}