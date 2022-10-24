using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
      
        // method returns the Villa that is updated.
        Task<Villa> UpdateAsync(Villa entity); 

    }

}
