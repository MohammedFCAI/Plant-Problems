namespace Plant_Problems.Service.Authentications.Interfaces
{
    public interface IUserRoleService
    {
        Task<ServiceResponse<string>> AssignRole(UserRole userRole);
    }
}
