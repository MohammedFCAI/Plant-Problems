using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace Plant_Problems.API.Helper
{
	public class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type.IsEnum)
			{
				var enumValues = Enum.GetValues(context.Type);
				var enumNames = new List<IOpenApiAny>();

				foreach (var value in enumValues)
				{
					var displayName = EnumExtensions.GetDisplayName(value as Enum);
					var enumString = new OpenApiString(displayName);
					enumNames.Add(enumString);
				}

				schema.Enum = enumNames;
			}
		}


		public static class EnumExtensions
		{
			public static string GetDisplayName(Enum value)
			{
				var fieldInfo = value.GetType().GetField(value.ToString());
				var displayAttribute = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

				return displayAttribute != null && displayAttribute.Length > 0
					? displayAttribute[0].Name
					: value.ToString();
			}
		}
	}
}
