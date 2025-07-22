using Ambev.DeveloperEvaluation.Domain.Repositories; // Mude para IUnitOfWork
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, GetSaleByIdQueryResponse?>
    {
        private readonly IUnitOfWork _unitOfWork; // Mude para IUnitOfWork

        public GetSaleByIdQueryHandler(IUnitOfWork unitOfWork) // Mude para IUnitOfWork
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetSaleByIdQueryResponse?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.Sales.GetByIdWithItemsAsync(request.Id, cancellationToken);

            if (sale is null)
            {
                return null;
            }

            return new GetSaleByIdQueryResponse
            {
                Id = sale.Id,
                SaleDate = sale.SaleDate,
                Customer = sale.Customer,
                Branch = sale.Branch,
                TotalAmount = sale.TotalAmount,
                IsCancelled = sale.IsCancelled,
                Items = sale.Items.Select(item => new SaleItemResponse
                {
                    Id = item.Id,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    TotalItemAmount = item.TotalItemAmount
                }).ToList()
            };
        }
    }
}
