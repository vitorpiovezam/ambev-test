using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    // A Query é o "pacote" que o MediatR vai enviar.
    // IRequest<T> indica que esta query espera uma resposta do tipo 'List<GetAllSalesQueryResponse>'.
    public class GetAllSalesQuery : IRequest<List<GetAllSalesQueryResponse>>
    {
    }

    // DTO que representa como cada venda será retornada na lista.
    public class GetAllSalesQueryResponse
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public int NumberOfItems { get; set; }
    }
}
