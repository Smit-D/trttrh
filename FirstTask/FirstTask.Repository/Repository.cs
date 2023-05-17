using FirstTask.Entities.Data;
using FirstTask.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FirstTaskDBContext _db;
        internal DbSet<T> dbset;
        public Repository(FirstTaskDBContext db)
        {
            _db = db;
            dbset = _db.Set<T>();
        }

        public async Task SaveDbAsync()
        {
           await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = dbset;
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }
      /*  public async Task<IEnumerable<T>> GetListByIdAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            return await query.Where(filter).ToListAsync();
        }*/
    }
}
