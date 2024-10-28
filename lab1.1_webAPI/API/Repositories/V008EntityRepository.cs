using Data.Model;
using Data.Contexts;

namespace API.Repositories
{
    public class V008EntityRepository : GenericRepository<V008Entity>
    {
        public V008EntityRepository(ApplicationContext context) : base(context) {}
    }
}