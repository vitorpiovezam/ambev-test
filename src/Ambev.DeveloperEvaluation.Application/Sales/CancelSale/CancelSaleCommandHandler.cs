using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelSaleCommandHandler> _logger;

        public CancelSaleCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelSaleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando processo de cancelamento para a venda {SaleId}", request.SaleId);

            var sale = await _unitOfWork.Sales.GetByIdAsync(request.SaleId, cancellationToken);

            if (sale is null)
            {
                _logger.LogWarning("Venda {SaleId} não encontrada para cancelamento.", request.SaleId);
                return false;
            }

            sale.IsCancelled = true;

            await _unitOfWork.Sales.UpdateAsync(sale);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Venda {SaleId} cancelada com sucesso.", request.SaleId);

            return true;
        }
    }
}
