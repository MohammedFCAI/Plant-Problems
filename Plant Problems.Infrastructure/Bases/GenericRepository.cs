using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Plant_Problems.Infrastructure.Bases
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public virtual async Task<T> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public virtual async Task AddRangeAsync(ICollection<T> entities)
		{
			await _context.Set<T>().AddRangeAsync(entities);
			await _context.SaveChangesAsync();
		}

		public IDbContextTransaction BeginTransaction()
		{
			return _context.Database.BeginTransaction();
		}

		public void Commit()
		{
			_context.Database.CommitTransaction();
		}

		public virtual async Task DeleteAsnc(T entity)
		{
			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task DeleteRangeAsync(ICollection<T> entities)
		{
			foreach (var entity in entities)
			{
				_context.Entry(entity).State = EntityState.Deleted;
			}
			await _context.SaveChangesAsync();
		}

		public virtual async Task<T> GetByIdAsync(Guid id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public int GetCount()
		{
			return _context.Set<T>().Count();
		}

		public IQueryable<T> GetTableAsTracking()
		{
			return _context.Set<T>().AsQueryable();
		}

		public IQueryable<T> GetTableNoTracking()
		{
			return _context.Set<T>().AsNoTracking().AsQueryable();
		}

		public void RoleBack()
		{
			_context.Database.RollbackTransaction();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public virtual async Task UpdatAsync(T entity)
		{
			_context.Set<T>().Update(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task UpdateRangeAsync(ICollection<T> entities)
		{
			_context.Set<T>().UpdateRange(entities);
			await _context.SaveChangesAsync();
		}
	}
}
