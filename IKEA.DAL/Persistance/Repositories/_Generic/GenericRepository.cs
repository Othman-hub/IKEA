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

        public IQueryable<T> GetAll(bool WithNoTracking = true)
            => WithNoTracking ? DbContext.Set<T>().AsNoTracking() : DbContext.Set<T>();

        public async Task<T?> GetById(int id) => await DbContext.Set<T>().FindAsync(id);

        public void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            
        }
        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
            
        }
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            DbContext.Set<T>().Update(entity);
            
        }
    }
}
