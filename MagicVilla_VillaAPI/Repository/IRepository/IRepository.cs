using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{

    // NOTE:
    // We have created this GENERIC Interface so that it can be used by ANY Repository.
    // Most if not all repositories will have the methods defined here (CRUD) and so instead
    // of putting them into each Repository implementation,
    // we will put them here.

    public interface IRepository<T> where T : class
    {
        // parameter Expression<Func<Villa>> is a LINQ to be passed to the method

        // Also, when we are GETTING, it is possible that FILTER is not supplied (NULL)
        // so we make it nullable here.

        // , string? includeProperties = null ->>> This means "Hey, I need this particular navigation
        // property to be loaded.". Also, this particular property may not be needed all the time so we
        // will make it Nullable here using the ?.
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);

        Task CreateAsync(T entity); // we use Task here as we want to use Async

        Task RemoveAsync(T entity);

        Task SaveAsync();
    }
}
