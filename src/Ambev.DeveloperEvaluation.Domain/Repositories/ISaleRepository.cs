using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<Sale?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
