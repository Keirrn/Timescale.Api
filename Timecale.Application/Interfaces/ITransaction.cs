namespace Timecale.Application.Interfaces;

public interface ITransaction
{
    Task BeginTransactionAsync(
        CancellationToken cancellationToken);

    Task CommitTransactionAsync(
        CancellationToken cancellationToken);

    Task RollbackTransactionAsync(
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}