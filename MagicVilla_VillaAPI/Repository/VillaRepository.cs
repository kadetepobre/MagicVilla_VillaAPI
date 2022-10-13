using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext mDbContext;

        public VillaRepository(ApplicationDbContext dbContext)
        {
            mDbContext = dbContext; 

        }


        public async Task CreateAsync(Villa entity)
        {
            await mDbContext.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetVillaAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = mDbContext.Villas;

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

            return await query.FirstOrDefaultAsync(); // Only at this point the query will be executed.
        
        }

        public async Task<List<Villa>> GetAllVillasAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = mDbContext.Villas;

            // When we work on IQueryable, it DOES NOT GET EXECUTED RIGHT AWAY.
            // So we can build on the query first.
            
            if(filter != null)
            {
                query = query.Where(filter);

            }

            return await query.ToListAsync(); // Only at this point the query will be executed.
        }

        public async Task RemoveAsync(Villa entity)
        {
            mDbContext.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            mDbContext.Villas.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await mDbContext.SaveChangesAsync();
        }
    }
}
