using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext DbContext;
        public DepartmentRepository(ApplicationDbContext dbContext) => DbContext = dbContext;

        public IEnumerable<Department> GetAll(bool WithNoTracking = true)
            => WithNoTracking ? DbContext.Departments.AsNoTracking().ToList() : DbContext.Departments.ToList();

        public Department? GetById(int id) => DbContext.Departments.Find(id);

        public int Add(Department department)
        {
            DbContext.Departments.Add(department);
            return DbContext.SaveChanges();
        }
        public int Update(Department department)
        {
            DbContext.Departments.Update(department);
            return DbContext.SaveChanges();
        }
        public int Delete(Department department)
        {
            DbContext.Departments.Remove(department);
            return DbContext.SaveChanges();
        }
    }
}
