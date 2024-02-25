using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plant_Problems.Data.Models;

namespace Plant_Problems.Infrastructure
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//SeedRoles(modelBuilder);
		}

		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<SavedPost> SavedPosts { get; set; }

		private static void SeedRoles(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityRole>().HasData
				(
					new IdentityRole() { Name = "Admin", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN" },
					new IdentityRole() { Name = "User", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER" }
				);
		}
	}
}