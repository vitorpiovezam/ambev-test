using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.Sales.GetByIdAsync(request.Id, cancellationToken);

            if (sale is null || sale.IsCancelled)
            {
                return false;
            }

            sale.Customer = request.Customer;
            sale.Branch = request.Branch;

            await _unitOfWork.Sales.UpdateAsync(sale);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
