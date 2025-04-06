using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories.Employees
{
    public class EmployeeRepsitory : GenericRepository<Employee>, IEmployeeRepositoris
    {
        private readonly ApplicationDbContext DbContext;
        public EmployeeRepsitory(ApplicationDbContext dbContext) : base(dbContext)
        => DbContext = dbContext;
    }
}
