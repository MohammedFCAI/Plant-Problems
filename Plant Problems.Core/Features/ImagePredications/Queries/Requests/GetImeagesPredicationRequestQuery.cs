namespace Plant_Problems.Core.Features.ImagePredications.Queries.Requests
{
	public class GetImeagesPredicationRequestQuery : IRequest<Response<List<ImagePredication>>>
	{
		public string UserId { get; set; }

		public GetImeagesPredicationRequestQuery(string userId)
		{
			UserId = userId;
		}
	}
}
