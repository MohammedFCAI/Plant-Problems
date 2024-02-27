namespace Plant_Problems.Core.Features.ImagePredications.Commands.Requests
{
	public class DeleteAllImagePredicationsRequestCommand : IRequest<Response<string>>
	{
		public string UserId { get; set; }

		public DeleteAllImagePredicationsRequestCommand(string userId)
		{
			UserId = userId;
		}
	}
}
