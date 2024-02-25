namespace Plant_Problems.Core.Bases
{
	public class ResponseHandler
	{
		public ResponseHandler()
		{

		}
		public Response<T> Deleted<T>(string? message = null)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Succeed = true,
				Message = message == null ? "Deleted.." : message
			};
		}

		public Response<T> Success<T>(T entity, string message = null, int count = 0)
		{
			return new Response<T>()
			{
				Count = count,
				Data = entity,
				StatusCode = System.Net.HttpStatusCode.OK,
				Succeed = true,
				Message = message == null ? "Success.." : message
			};
		}


		public Response<T> Unauthorized<T>(T entity)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.Unauthorized,
				Succeed = true,
				Message = "Un Authorized..!"
			};
		}

		public Response<T> BadRequest<T>(string message = null)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.BadRequest,
				Succeed = false,
				Message = message == null ? "Bad Request :(" : message
			};
		}

		public Response<T> UnprocessableEntity<T>(string message = null)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
				Succeed = false,
				Message = message == null ? "Unprocessable Entity" : message
			};
		}

		public Response<T> NotFound<T>(string message = null)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.NotFound,
				Succeed = false,
				Message = message == null ? "NotFound :(" : message
			};
		}

		public Response<T> Created<T>(T entity)
		{
			return new Response<T>()
			{
				Data = entity,
				StatusCode = System.Net.HttpStatusCode.Created,
				Succeed = true,
				Message = "Created..",
			};
		}

		public Response<T> Empty<T>(string message = null)
		{
			return new Response<T>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Succeed = true,
				Message = message == null ? "Empty..!" : message
			};
		}
	}
}
