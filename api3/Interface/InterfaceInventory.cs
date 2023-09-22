using api3.Models;

namespace api3.Interface
{
    public interface InterfaceInventory
    {
        ICollection<Inventory> GetInventory(); // get
        //Facturas GetFacturas(int id);
        bool UpdateInventory(int InventoryID, Inventory inventory); // put
        bool save(); // guardar
        bool InventoryExist(int idInventory); // put y post

        bool CreateInventory(Inventory inventory); //post
    }
}
