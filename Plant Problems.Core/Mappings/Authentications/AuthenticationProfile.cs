namespace Plant_Problems.Core.Mappings.Authentications
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            RegisterUserMapping();
            LoginUserMapping();
            AssignRoleMapping();
            ChangePasswordMapping();
        }

        private void RegisterUserMapping()
        {
            CreateMap<RegisterRequest, Register>();



            CreateMap<RegisterRequest, ApplicationUser>().ReverseMap();
        }

        private void LoginUserMapping()
        {
            CreateMap<LoginRequest, Login>().ReverseMap();
        }

        private void AssignRoleMapping()
        {
            CreateMap<UserRoleRequest, UserRole>().ReverseMap();
        }

        private void ChangePasswordMapping()
        {
            CreateMap<ChangeUserPasswordRequest, ApplicationUser>().ReverseMap();
        }
    }
}
