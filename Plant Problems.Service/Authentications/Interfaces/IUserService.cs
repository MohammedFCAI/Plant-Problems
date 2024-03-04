namespace Plant_Problems.Service.Authentications.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<ApplicationUser>> CreateUser(Register model);
        Task<ServiceResponse<ApplicationUser>> SignIn(Login model);
        Task<ServiceResponse<ApplicationUser>> ChangePassword(string username, string oldPassword, string newPassword, string confirmNewPassword);
        Task<ServiceResponse<string>> ForgetPassword(ForgetPassword request);
        public void Detach(ApplicationUser user);
        public Task<ServiceResponse<ApplicationUser>> GetUserById(string id);
        Task<ServiceResponse<List<ApplicationUser>>> GetUsersList();
        Task<ServiceResponse<ApplicationUser>> DeleteUser(string userId, string password);


        // New
        Task<ServiceResponse<TokensResponse>> LoginWithJwt(Login model);
        Task<ServiceResponse<LoginResponse>> RenewAccessToken(LoginResponse tokens);
    }
}
