﻿namespace Plant_Problems.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
