namespace Plant_Problems.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IImageManager, ImageManager>();
            services.AddTransient<IImagePredicationService, ImagePredicationService>();
            services.AddLogging();

            return services;
        }
    }
}
