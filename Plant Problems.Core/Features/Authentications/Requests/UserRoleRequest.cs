using Plant_Problems.Data.Models.Authentications;

namespace Plant_Problems.Core.Features.Authentications.Requests
{
	public class UserRoleRequest : IRequest<Response<string>>
	{
		public string Username { get; set; }
		public Role Role { get; set; }
	}
}
