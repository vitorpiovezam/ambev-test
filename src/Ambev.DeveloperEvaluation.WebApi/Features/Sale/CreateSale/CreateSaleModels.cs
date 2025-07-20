namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string Customer { get; set; }
        public string Branch { get; set; }
        public List<SaleItemRequest> Items { get; set; }
    }

    public class SaleItemRequest
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreateSaleResponse
    {
        public Guid SaleId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
