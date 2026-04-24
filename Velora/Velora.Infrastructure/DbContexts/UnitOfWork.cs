using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Velora.Domain.Contracts;

namespace Velora.Infrastructure.DbContexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await BeginTransactionAsync();
                try
                {
                    await action();
                    await _dbContext.SaveChangesAsync();
                    await CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await RollbackTransactionAsync();
                    throw;
                }
            });
        }

        private async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        private async Task CommitTransactionAsync()
        {
            if(_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        private async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
