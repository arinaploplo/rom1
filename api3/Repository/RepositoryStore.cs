using api3.Interface;
using api3.Models;

namespace api3.Repository
{
    public class RepositoryStore : InterfaceStore
    {
        private readonly PgAdminContext _context;
        public RepositoryStore(PgAdminContext context)
        {
            _context = context;
        }
        public bool CreateStore(Store Store)
        {
            throw new NotImplementedException();
        }

        public ICollection<Store> GetStore()
        {
            return _context.Stores.OrderBy(H => H.IdStore).ToList();

        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool StoreExist(int IdStore)
        {
            return _context.Stores.Any(p => p.IdStore == IdStore);

        }

        public bool UpdateStore(int StoreID, Store Store)
        {
            _context.Update(Store);
            return save();

        }
    }
}
