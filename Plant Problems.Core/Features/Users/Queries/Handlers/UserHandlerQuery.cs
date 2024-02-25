using Plant_Problems.Core.Features.Users.Queries.Requests;
using Plant_Problems.Service.Authentications.Interfaces;

namespace Plant_Problems.Core.Features.Users.Queries.Handlers
{
	public class UserHandlerQuery : ResponseHandler, IRequestHandler<GetUsersListRequestQuery, Response<List<ApplicationUser>>>
		, IRequestHandler<GetUserByIdRequestQuery, Response<ApplicationUser>>
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UserHandlerQuery(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<Response<List<ApplicationUser>>> Handle(GetUsersListRequestQuery request, CancellationToken cancellationToken)
		{
			var usersResponse = await _userService.GetUsersList();
			var users = usersResponse.Entities;
			if (users == null)
				users = new List<ApplicationUser>();

			return Success(users, usersResponse.Message, users.Count());
		}

		public async Task<Response<ApplicationUser>> Handle(GetUserByIdRequestQuery request, CancellationToken cancellationToken)
		{
			var userResponse = await _userService.GetUserById(request.UserId);
			var user = userResponse.Entities;
			if (!userResponse.Success)
				return NotFound<ApplicationUser>(userResponse.Message);

			return Success(user, userResponse.Message, 1);
		}
	}
}
