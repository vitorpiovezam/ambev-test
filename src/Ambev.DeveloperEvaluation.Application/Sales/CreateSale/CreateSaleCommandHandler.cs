using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSaleCommandHandler> _logger;

        public CreateSaleCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateSaleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando processo de criação de venda para o cliente {Customer}", request.Customer);
            
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = request.Customer,
                Branch = request.Branch,
                IsCancelled = false
            };

            foreach (var itemInput in request.Items)
            {
                if (itemInput.Quantity > 20)
                    throw new InvalidOperationException("Não é possível vender mais de 20 itens iguais.");

                decimal discountPercentage = 0;
                if (itemInput.Quantity >= 10)
                {
                    discountPercentage = 0.20m; 
                }
                else if (itemInput.Quantity >= 4)
                {
                    discountPercentage = 0.10m;
                }

                var totalItemPrice = itemInput.Quantity * itemInput.UnitPrice;
                var discountValue = totalItemPrice * discountPercentage;
                var finalItemPrice = totalItemPrice - discountValue;

                sale.Items.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    Product = itemInput.Product,
                    Quantity = itemInput.Quantity,
                    UnitPrice = itemInput.UnitPrice,
                    Discount = discountValue,
                    TotalItemAmount = finalItemPrice,
                    SaleId = sale.Id
                });
            }

            sale.TotalAmount = sale.Items.Sum(item => item.TotalItemAmount);

            await _unitOfWork.Sales.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Venda {SaleId} criada com sucesso com o valor total de {TotalAmount}", sale.Id, sale.TotalAmount);

            return new CreateSaleCommandResponse
            {
                SaleId = sale.Id,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}
