using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {

        // parameter Expression<Func<Villa>> is a LINQ to be passed to the method
        Task<List<Villa>> GetAllVillasAsync(Expression<Func<Villa, bool >> filter = null);

        Task<Villa> GetVillaAsync(Expression<Func<Villa, bool>> filter = null, bool tracked=true);

        Task CreateAsync(Villa entity); // we use Task here as we want to use Async
        
        Task RemoveAsync(Villa entity);

        Task UpdateAsync(Villa entity); 

        Task SaveAsync();



    }

}
