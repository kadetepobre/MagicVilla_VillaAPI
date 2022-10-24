using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
      
        // method returns the Villa that is updated.
        Task<VillaNumber> UpdateAsync(VillaNumber entity); 

    }

}
