using api3.Models;

namespace api3.Interface
{
    public interface InterfaceEmployee
    {
        ICollection<Employee> GetEmployee(); // get
        //Facturas GetFacturas(int id);
        bool UpdateEmployee(int EmployeeID, Employee employee); // put
        bool save(); // guardar
        bool EmployeeExist(int idEmployee); // put y post
        bool CreateEmployee(Employee employee); //post
    }
}
