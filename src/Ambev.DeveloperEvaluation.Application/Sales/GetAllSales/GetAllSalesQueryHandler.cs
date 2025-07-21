using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, List<GetAllSalesQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllSalesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllSalesQueryResponse>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _unitOfWork.Sales.GetAllAsync();

            var response = sales.Select(sale => new GetAllSalesQueryResponse
            {
                Id = sale.Id,
                SaleDate = sale.SaleDate,
                Customer = sale.Customer,
                Branch = sale.Branch,
                TotalAmount = sale.TotalAmount,
                IsCancelled = sale.IsCancelled,
                NumberOfItems = sale.Items.Count
            }).ToList();

            return response;
        }
    }
}
