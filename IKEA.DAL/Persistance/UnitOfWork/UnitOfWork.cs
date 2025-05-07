using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repositories.Departments;
using IKEA.DAL.Persistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public IDepartmentRepository departmentRepository { get; }
        public IEmployeeRepositoris employeeRepositoris { get; }
       

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            departmentRepository = new DepartmentRepository(this.dbContext);
            employeeRepositoris = new EmployeeRepsitory(this.dbContext);
        }

        public async Task<int> Complete()
        => await dbContext.SaveChangesAsync();
    }
}
