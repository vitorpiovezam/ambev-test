using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultContext _context;
        public IUserRepository Users { get; }
        public ISaleRepository Sales { get; }

        public UnitOfWork(DefaultContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Sales = new SaleRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
