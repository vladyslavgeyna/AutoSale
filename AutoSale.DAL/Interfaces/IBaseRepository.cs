namespace AutoSale.DAL.Interfaces
{
    public interface IBaseRepository<T, K>
    {
        Task<T> InsertAsync(T entity);
        
        IQueryable<T> Select();

        Task DeleteAsync(T entity);
        
        Task<T> UpdateAsync(T entity);

        Task<T?> GetByIdAsync(K id);
    }
}