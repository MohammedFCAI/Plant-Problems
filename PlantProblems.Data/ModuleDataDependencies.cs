namespace Plant_Problems.Data
{
    public static class ModuleDataDependencies
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<EnumSchemaFilter>();
            });
            return services;
        }
    }
}
