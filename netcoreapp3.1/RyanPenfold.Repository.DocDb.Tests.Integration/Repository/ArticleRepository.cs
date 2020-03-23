// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleRepository.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using RyanPenfold.Repository.DocDb.Tests.Integration.Model.RepositoryInterfaces;
using RyanPenfold.Repository.DocDb.Tests.Integration.Models;
using RyanPenfold.Repository.DocDb.Tests.Integration.Repository.Mappings;

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Repository
{
    /// <summary>
    /// Provides data access functionality relating to <see cref="Article" /> objects.
    /// </summary>
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleRepository"/> class.
        /// </summary>
        public ArticleRepository() : base(new ArticleMap())
        {
        }
    }
}