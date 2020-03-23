// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsiteRepository.cs" company="RyanPenfold">
//   Copyright © RyanPenfold. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using RyanPenfold.Repository.DocDb.Tests.Integration.Model.RepositoryInterfaces;
using RyanPenfold.Repository.DocDb.Tests.Integration.Models;
using RyanPenfold.Repository.DocDb.Tests.Integration.Repository.Mappings;

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Repository
{
    /// <summary>
    /// Provides data access functionality relating to <see cref="Website" /> objects.
    /// </summary>
    public class WebsiteRepository : BaseRepository<Website>, IWebsiteRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteRepository"/> class.
        /// </summary>
        public WebsiteRepository() : base(new WebsiteMap())
        {
        }
    }
}