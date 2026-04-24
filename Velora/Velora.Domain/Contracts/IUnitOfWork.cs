namespace Velora.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
