using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
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
                    discountPercentage = 0.20m; // 20%
                }
                else if (itemInput.Quantity >= 4)
                {
                    discountPercentage = 0.10m; // 10%
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

            return new CreateSaleCommandResponse
            {
                SaleId = sale.Id,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}
