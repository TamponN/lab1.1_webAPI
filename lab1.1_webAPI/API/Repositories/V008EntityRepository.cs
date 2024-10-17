using Data.Model;

namespace API.Repositories
{
    public class V008EntityRepository : GenericRepository<V008Entity>
    {
        public V008EntityRepository(ApplicationContext context) : base(context) {}
    }
}