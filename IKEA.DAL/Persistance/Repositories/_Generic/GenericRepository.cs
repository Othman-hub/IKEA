using IKEA.DAL.Models;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenrecRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext DbContext;
        public GenericRepository(ApplicationDbContext dbContext) => DbContext = dbContext;

        public IEnumerable<T> GetAll(bool WithNoTracking = true)
            => WithNoTracking ? DbContext.Set<T>().Where(D => !D.IsDeleted).AsNoTracking().ToList() : DbContext.Set<T>().Where(D => !D.IsDeleted).ToList();

        public T? GetById(int id) => DbContext.Set<T>().Find(id);

        public int Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return DbContext.SaveChanges();
        }
        public int Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
            return DbContext.SaveChanges();
        }
        public int Delete(T entity)
        {
            entity.IsDeleted = true;
            DbContext.Set<T>().Update(entity);
            return DbContext.SaveChanges();
        }
    }
}
