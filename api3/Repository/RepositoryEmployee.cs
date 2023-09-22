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
            throw new NotImplementedException();
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
    }
}
