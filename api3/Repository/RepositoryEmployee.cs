using api3.Interface;
using api3.Models;

namespace api3.Repository
{
    public class RepositoryEmployee : InterfaceEmployee
    {
        private readonly PgAdminContext _context;
        public RepositoryEmployee(PgAdminContext context)
        {
            _context = context;
        }
        public bool CreateEmployee(Employee Employee)
        {
            _context.Add(Employee);
            return save();
        }

        public ICollection<Employee> GetEmployee()
        {
            return _context.Employees.OrderBy(H => H.IdEmployee).ToList();

        }

        public bool save()
        {
            
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool EmployeeExist(int idEmployee)
        {
            return _context.Employees.Any(p => p.IdEmployee == idEmployee);

        }

        public bool UpdateEmployee(int EmployeeID, Employee employee)
        {
            _context.Update(employee);
            return save();

        }

        public bool DeleteEmployee(Employee Employee)
        {
            _context.Remove(Employee);
            return save();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.Where(e => e.IdEmployee == id).FirstOrDefault();
        }

        public int GetNextEmployeeId()
        {
            var lastEmployee = _context.Inventories
                .OrderByDescending(e => e.IdEmployee)
                .FirstOrDefault();

            if (lastEmployee != null)
            {
                // Se encontró el último registro, devuelve el siguiente ID.
                int id = Convert.ToInt32(lastEmployee.IdEmployee + 1);
                return id;
            }
            else
            {
                // No se encontraron registros en la tabla, devuelve 1 como el primer ID.
                return 1;
            }
        }
    }
}
