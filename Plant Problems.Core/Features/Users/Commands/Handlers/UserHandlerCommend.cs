namespace Plant_Problems.Core.Features.Users.Commands.Handlers
{
    public class UserHandlerCommend : ResponseHandler, IRequestHandler<DeleteUserRequestCommand, Response<string>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserHandlerCommend(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(DeleteUserRequestCommand request, CancellationToken cancellationToken)
        {
            var userRespone = await _userService.DeleteUser(request.UserId, request.Password);
            if (!userRespone.Success)
                return NotFound<string>(userRespone.Message);

            return Deleted<string>(userRespone.Message);
        }
    }
}
