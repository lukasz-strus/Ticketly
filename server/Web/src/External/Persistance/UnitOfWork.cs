using Application.Core.Abstractions.Data;

namespace Persistance;

internal sealed class UnitOfWork(
    ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            //TODO: Log exception
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
