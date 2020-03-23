using RyanPenfold.BusinessBase.Infrastructure;
using RyanPenfold.Repository.DocDb.Tests.Integration.Models;

namespace RyanPenfold.Repository.DocDb.Tests.Integration.Model.RepositoryInterfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
    }
}