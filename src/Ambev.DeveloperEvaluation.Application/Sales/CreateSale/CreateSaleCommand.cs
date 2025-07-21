using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleCommandResponse>
    {
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public List<SaleItemCommand> Items { get; set; } = new List<SaleItemCommand>();
    }

    public class SaleItemCommand
    {
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreateSaleCommandResponse
    {
        public Guid SaleId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
