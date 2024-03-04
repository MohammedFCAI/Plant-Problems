namespace Plant_Problems.Infrastructure.Bases
{
    public interface IGenericRepository<T> where T : class
    {
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<T> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RoleBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdatAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsnc(T entity);
        int GetCount();
    }
}
