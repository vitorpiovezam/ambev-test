using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<SaleItemRequest, SaleItemCommand>();

            CreateMap<CreateSaleCommandResponse, CreateSaleResponse>();
        }
    }
}
