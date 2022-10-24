using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {

        
        private readonly ApplicationDbContext mDbContext;

        // NOTE: The DBContext that we receive in this CTOR, we need to pass it 
        // to REPOSITORY as well, which is the base class here as it also
        // expects a DBContext
        public VillaNumberRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            mDbContext = dbContext;
        }


        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            mDbContext.VillaNumbers.Update(entity);
            await mDbContext.SaveChangesAsync();

            return entity;
        }

      
    }
}
