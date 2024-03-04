namespace Plant_Problems.Infrastructure.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        void Detach(ApplicationUser user);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<List<ApplicationUser>> GetAllAsync();
    }
}
