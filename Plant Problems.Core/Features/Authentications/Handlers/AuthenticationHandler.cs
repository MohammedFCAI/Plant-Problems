namespace Plant_Problems.Core.Features.Authentications.Handlers
{
    public class AuthenticationHandler : ResponseHandler, IRequestHandler<RegisterRequest, Response<string>>
        , IRequestHandler<LoginRequest, Response<string>>
        , IRequestHandler<UserRoleRequest, Response<string>>
        , IRequestHandler<ChangeUserPasswordRequest, Response<string>>
        , IRequestHandler<SendEmailRequest, Response<string>>
        , IRequestHandler<ForgetPasswordRequest, Response<string>>

    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IEmailService _emailService;
        public AuthenticationHandler(IMapper mapper, IUserService userService, IUserRoleService userRoleService, IEmailService emailService)
        {
            _mapper = mapper;
            _userService = userService;
            _userRoleService = userRoleService;
            _emailService = emailService;
        }

        public async Task<Response<string>> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var registerUser = _mapper.Map<Register>(request);

            var result = await _userService.CreateUser(registerUser);

            if (!result.Success)
                return BadRequest<string>(result.Message);

            return Success(result.Message);
        }

        public async Task<Response<string>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Login>(request);

            var result = await _userService.SignIn(user);

            if (!result.Success)
                return BadRequest<string>(result.Message);

            return Success(result.Message);
        }

        public async Task<Response<string>> Handle(UserRoleRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserRole>(request);

            var result = await _userRoleService.AssignRole(user);

            if (!result.Success)
                return BadRequest<string>(result.Message);

            return Success(result.Message);
        }

        public async Task<Response<string>> Handle(ChangeUserPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.ChangePassword(request.Username, request.Password, request.NewPassword, request.ConfirmNewPassword);

            if (!result.Success)
                return BadRequest<string>(result.Message);

            return Success(result.Message);
        }

        public async Task<Response<string>> Handle(SendEmailRequest request, CancellationToken cancellationToken)
        {
            var message = new Message(request.To, request.Subject, request.Content);

            var response = await _emailService.SendEmailAsync(message);

            return Success(response.Message);
        }

        public async Task<Response<string>> Handle(ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            var forgetPassword = new ForgetPassword() { Email = request.Email };
            var userResponse = await _userService.ForgetPassword(forgetPassword);
            if (!userResponse.Success)
                return NotFound<string>(userResponse.Message);

            return Success(userResponse.Message);
        }
    }
}
