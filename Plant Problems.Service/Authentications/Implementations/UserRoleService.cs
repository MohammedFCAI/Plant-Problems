namespace Plant_Problems.Service.Authentications.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResponse<string>> AssignRole(UserRole userRole)
        {
            var user = await _userManager.FindByNameAsync(userRole.Username);

            if (user == null)
                return new ServiceResponse<string> { Entities = "", Success = false, Message = "Invalid username!" };
            var role = userRole.Role.ToString().ToUpper();

            var userHasRole = await _userManager.GetRolesAsync(user);
            if (!userHasRole.IsNullOrEmpty())
            {
                var userRoleName = userHasRole[0].ToUpper();
                if (userRoleName == role)
                    return new ServiceResponse<string> { Entities = "", Success = false, Message = "User has already this role!" };
            }

            await _userManager.AddToRoleAsync(user, role);
            return new ServiceResponse<string> { Entities = "", Success = true, Message = "Role Assigned Successfully." };
        }
    }
}
