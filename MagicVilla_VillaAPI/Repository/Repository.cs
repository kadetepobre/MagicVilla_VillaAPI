using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext mDbContext;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            mDbContext = dbContext;

            // When VillaNumber records are retrieved, instruct EF to also Include
            // related (via Foreign key) Villa records
            mDbContext.VillaNumbers.Include(m => m.Villa).ToList();

            this.dbSet = mDbContext.Set<T>(); //ex. Set <Villa>
        }


        //public async Task CreateAsync(Villa entity)
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }


        //
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            // When we work on IQueryable, it DOES NOT GET EXECUTED RIGHT AWAY.
            // So we can build on the query first.
            if (filter != null)
            {
                query = query.Where(filter);

            }


            // this is to be used when there are Foreign Key - related records which we are loading
            // In this case, we want to include a VILLA record (via FK) with each VillaNumber record
            // we are retrieving

            if(includeProperties != null)
            {
                // example, we have several include properties (in this case though, only the VILLA)
                //i.e. "Villa, AnotherInclude1, AnotherInclude2..."
                foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync(); // Only at this point the query will be executed.

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // When we work on IQueryable, it DOES NOT GET EXECUTED RIGHT AWAY.
            // So we can build on the query first.

            if (filter != null)
            {
                query = query.Where(filter);

            }

            // this is to be used when there are Foreign Key - related records which we are loading
            // In this case, we want to include a VILLA record (via FK) with each VillaNumber record
            // we are retrieving

            if (includeProperties != null)
            {
                // example, we have several include properties (in this case though, only the VILLA)
                //i.e. "Villa, AnotherInclude1, AnotherInclude2..."
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.ToListAsync(); // Only at this point the query will be executed.
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await mDbContext.SaveChangesAsync();
        }
    }
}
