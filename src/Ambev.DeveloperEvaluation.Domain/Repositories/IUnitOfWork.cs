namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISaleRepository Sales { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
