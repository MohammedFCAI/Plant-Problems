namespace Plant_Problems.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection service)
        {
            service.AddTransient<IPostRepository, PostRepository>();
            service.AddTransient<ICommentRepository, CommentRepository>();
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddTransient<IUserRepository, UserRepository>();
            service.AddTransient<ISavedPostRepository, SavedPostRepository>();
            service.AddTransient<IImagePredicationRepositry, ImagePredicationRepository>();
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return service;
        }
    }
}
