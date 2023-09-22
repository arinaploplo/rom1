using api3.Interface;
using api3.Models;

namespace api3.Repository
{
    public class RepositoryInventory : InterfaceInventory
    {
        private readonly PgAdminContext _context;
        public RepositoryInventory(PgAdminContext context)
        {
            _context = context;
        }
        public bool CreateInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public ICollection<Inventory> GetInventory()
        {
            return _context.Inventories.OrderBy(H => H.IdInventory).ToList();

        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool InventoryExist(int idInventory)
        {
            return _context.Inventories.Any(p => p.IdInventory == idInventory);

        }

        public bool UpdateInventory(int InventoryID, Inventory inventory)
        {
            _context.Update(inventory);
            return save();

        }
    }
}
